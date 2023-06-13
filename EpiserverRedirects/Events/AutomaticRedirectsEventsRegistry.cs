using System.Linq;
using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model;
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
                if (!(_contentRepository.Get<IContentData>(e.ContentLink, language.Culture) is PageData pageData))
                {
                    continue;
                }

                var originalParentUrl = GetContentUrl(originalParent, language.Culture.Name, false);

                if (originalParentUrl == null)
                {
                    continue;
                }

                var oldUrl = originalParentUrl + pageData.URLSegment;

                var currentUrl = GetContentUrl(e.ContentLink, language.Culture.Name, false);
                if (oldUrl == UrlPath.NormalizePath(currentUrl))
                {
                    continue;
                }
                
                _systemRedirectsActions.AddRedirects(
                    pageData,
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

            var oldUrl = GetContentUrl(lastVersion.ContentLink, lastVersion.LanguageBranch);

            if (oldUrl == null)
            {
                return;
            }

            var newUrl = GetContentUrl(e.ContentLink, e.Content.LanguageBranch());

            if (oldUrl == newUrl)
            {
                return;
            }

            var lastVersionPageData = _contentRepository.Get<IContentData>(lastVersion.ContentLink) as PageData;

            if (lastVersionPageData == null)
            {
                return;
            }

            _systemRedirectsActions.AddRedirects(
                lastVersionPageData,
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

            var oldUrl = _urlResolver.GetUrl(e.ContentLink, null);
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

            var newUrl = _urlResolver.GetUrl(e.ContentLink, null);

            if (newUrl != oldUrl)
            {
                var pageData = _contentRepository.Get<IContentData>(e.ContentLink) as PageData;

                _systemRedirectsActions.AddRedirects(
                    pageData,
                    oldUrl,
                    SystemRedirectsHelper.GetCultureInfo(e),
                    SystemRedirectReason.SavedContent,
                    _redirectsOptions.SystemRedirectRulePriority);
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

        private string GetContentUrl(ContentReference contentReference, string language, bool validateTemplate = true)
        {
            var arguments = new VirtualPathArguments {ValidateTemplate = validateTemplate};

            return _urlResolver.GetVirtualPath(contentReference, language, arguments)?.VirtualPath;
        }
    }
}
