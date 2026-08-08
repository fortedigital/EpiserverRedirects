using System.Linq;
using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Resolver.Content;
using Forte.EpiserverRedirects.System;

namespace Forte.EpiserverRedirects.Events
{
    internal class AutomaticRedirectsEventsRegistry
    {
        private const string OldUrlKey = "OLD_URL";

        private readonly IContentEvents _contentEvents;
        private readonly IContentRepository _contentRepository;
        private readonly IContentVersionRepository _contentVersionRepository;
        private readonly ILanguageBranchRepository _languageBranchRepository;
        private readonly SystemRedirectsActions _systemRedirectsActions;
        private readonly UrlResolver _urlResolver;
        private readonly RedirectsOptions _redirectsOptions;

        public AutomaticRedirectsEventsRegistry(
            IContentEvents contentEvents,
            IContentRepository contentRepository,
            IContentVersionRepository contentVersionRepository,
            ILanguageBranchRepository languageBranchRepository,
            SystemRedirectsActions systemRedirectsActions,
            UrlResolver urlResolver,
            RedirectsOptions redirectsOptions)
        {
            _contentEvents = contentEvents;
            _contentRepository = contentRepository;
            _contentVersionRepository = contentVersionRepository;
            _languageBranchRepository = languageBranchRepository;
            _systemRedirectsActions = systemRedirectsActions;
            _urlResolver = urlResolver;
            _redirectsOptions = redirectsOptions;
        }

        public void RegisterEvents()
        {
            _contentEvents.MovedContent += MovedContentHandler;
            _contentEvents.PublishedContent += PublishedContentHandler;
            _contentEvents.SavingContent += SavingContentHandler;
            _contentEvents.SavedContent += SavedContentHandler;
            _contentEvents.DeletedContent += DeletedContentHandler;
        }

        private void MovedContentHandler(object sender, ContentEventArgs e)
        {
            if (EventsHandlersScopeConfiguration.IsAutomaticRedirectsDisabled)
            {
                return;
            }

            if (!(e.Content is IChangeTrackable))
            {
                return;
            }

            var originalParent = (e as MoveContentEventArgs)?.OriginalParent;

            if (originalParent == ContentReference.WasteBasket)
                // do not create when restoring, cause not need to do redirects from waste basket.
                // however, DO redirect when moving to waste basket, because restore may be to another place
            {
                return;
            }

            foreach (var language in _languageBranchRepository.ListEnabled())
            {
                var content = _contentRepository.Get<IContentData>(e.ContentLink, language.Culture) as IContent;
                if (content == null || !IsCmsOrCatalogContent(content))
                {
                    continue;
                }

                var routeSegment = (content as IRoutable)?.RouteSegment;
                if (routeSegment == null)
                {
                    continue;
                }

                var originalParentUrl = GetContentUrl(originalParent, language.Culture.Name, false);

                if (originalParentUrl == null)
                {
                    continue;
                }

                var oldUrl = originalParentUrl + routeSegment;

                var currentUrl = GetContentUrl(e.ContentLink, language.Culture.Name, false);
                if (oldUrl == UrlPath.NormalizePath(currentUrl))
                {
                    continue;
                }
                
                _systemRedirectsActions.AddRedirects(
                    content,
                    oldUrl,
                    language.Culture,
                    SystemRedirectReason.MovedContent,
                    _redirectsOptions.SystemRedirectRulePriority);
            }
        }

        private void PublishedContentHandler(object sender, ContentEventArgs e)
        {
            if (EventsHandlersScopeConfiguration.IsAutomaticRedirectsDisabled)
            {
                return;
            }

            var lastVersion = _contentVersionRepository
                .List(e.ContentLink, e.Content.LanguageBranch())
                .Where(p => p.Status == VersionStatus.PreviouslyPublished)
                .OrderByDescending(p => p.Saved)
                .FirstOrDefault();

            if (lastVersion == null)
            {
                return;
            }

            var oldUrl = GetContentUrl(lastVersion.ContentLink, lastVersion.LanguageBranch, false);

            if (oldUrl == null)
            {
                return;
            }

            var newUrl = GetContentUrl(e.ContentLink, e.Content.LanguageBranch(), false);

            if (oldUrl == newUrl)
            {
                return;
            }

            var lastVersionContent = _contentRepository.Get<IContentData>(lastVersion.ContentLink) as IContent;

            if (lastVersionContent == null || !IsCmsOrCatalogContent(lastVersionContent))
            {
                return;
            }

            _systemRedirectsActions.AddRedirects(
                lastVersionContent,
                oldUrl,
                SystemRedirectsHelper.GetCultureInfo(e),
                SystemRedirectReason.PublishedContent,
                _redirectsOptions.SystemRedirectRulePriority);
        }

        private void SavingContentHandler(object sender, ContentEventArgs e)
        {
            if (EventsHandlersScopeConfiguration.IsAutomaticRedirectsDisabled)
            {
                return;
            }

            var transition = ((SaveContentEventArgs) e).Transition;

            if (transition.CurrentStatus == VersionStatus.NotCreated)
            {
                return;
            }

            // create redirects only if page is unpublished
            // because child objects may have been already published so their URL changes
            if (_contentVersionRepository
                .List(e.ContentLink)
                .Any(p => p.Status == VersionStatus.Published || p.Status == VersionStatus.PreviouslyPublished))
            {
                return;
            }

            var oldUrl = GetContentUrl(e.ContentLink, null, false);
            e.Items.Add(OldUrlKey, oldUrl);
        }

        private void SavedContentHandler(object sender, ContentEventArgs e)
        {
            if (EventsHandlersScopeConfiguration.IsAutomaticRedirectsDisabled)
            {
                return;
            }

            var oldUrl = e.Items[OldUrlKey]?.ToString();

            if (oldUrl == null)
            {
                return;
            }

            var newUrl = GetContentUrl(e.ContentLink, null, false);

            if (newUrl != oldUrl)
            {
                var content = _contentRepository.Get<IContentData>(e.ContentLink) as IContent;

                if (content != null && IsCmsOrCatalogContent(content))
                {
                    _systemRedirectsActions.AddRedirects(
                        content,
                        oldUrl,
                        SystemRedirectsHelper.GetCultureInfo(e),
                        SystemRedirectReason.SavedContent,
                        _redirectsOptions.SystemRedirectRulePriority);
                }
            }

            e.Items.Remove(OldUrlKey);
        }

        private void DeletedContentHandler(object sender, ContentEventArgs e)
        {
            if (EventsHandlersScopeConfiguration.IsAutomaticRedirectsDisabled)
            {
                return;
            }

            _systemRedirectsActions.DeleteRedirects(e.ContentLink, ((DeleteContentEventArgs) e).DeletedDescendents);
        }

        private static bool IsCmsOrCatalogContent(IContent content) =>
            content is PageData || content.ContentLink.ProviderName == CommerceRedirectContentResolver.CatalogProviderKey;

        private string GetContentUrl(ContentReference contentReference, string language, bool validateTemplate = true)
        {
            var arguments = new VirtualPathArguments {ValidateTemplate = validateTemplate};

            return _urlResolver.GetVirtualPath(contentReference, language, arguments)?.VirtualPath;
        }
    }
}
