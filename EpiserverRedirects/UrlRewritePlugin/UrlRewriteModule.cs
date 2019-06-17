using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace Forte.EpiserverRedirects.UrlRewritePlugin
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class UrlRewriteModule : IInitializableModule
    {
        private const string _oldUrlKey = "OLD_URL";

        private Injected<UrlResolver> UrlResolver { get; set; }
        private Injected<IContentVersionRepository> ContentVersionRepository { get; set; }
        private Injected<IContentRepository> ContentRepository { get; set; }
        private Injected<ILanguageBranchRepository> LanguageBranchRepository { get; set; }

        public void Initialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent += MovedContentHandler;
            events.PublishedContent += PublishedConentHandler;
            events.SavingContent += SavingContentHandler;
            events.SavedContent += SavedContentHandler;
            events.DeletedContent += DeletedContentHandler;
        }

        private void PublishedConentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.AddAutomaticRedirects == false)
            {
                return;
            }

            var lastVersion = ContentVersionRepository.Service
                .List(e.ContentLink)
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

            var lastVersionPageData = ContentRepository.Service.Get<IContentData>(lastVersion.ContentLink) as PageData;
            if (lastVersionPageData == null)
            {
                return;
            }

            RedirectHelper.AddRedirects(lastVersionPageData, oldUrl, GetCultureInfo(e));
        }

        private string GetContentUrl(ContentReference contentReference, string language, bool validateTemplate = true)
        {
            var arguments = new VirtualPathArguments { ValidateTemplate = validateTemplate };
            return UrlResolver.Service.GetVirtualPath(contentReference, language, arguments)?.VirtualPath;
        }

        public void Uninitialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent -= MovedContentHandler;
            events.PublishedContent -= PublishedConentHandler;
            events.SavingContent -= SavingContentHandler;
            events.SavedContent -= SavedContentHandler;
            events.DeletedContent -= DeletedContentHandler;
        }

        private void MovedContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.AddAutomaticRedirects == false)
                return;

            if (!(e.Content is IChangeTrackable)) return;

            var originalParent = (e as MoveContentEventArgs)?.OriginalParent;

            if (originalParent == ContentReference.WasteBasket)
            {
                // do not create when restoring, cause not need to do redirects from waste basket.
                // however, DO redirect when moving to waste basket, because restore may be to another place 
                return;
            }

            foreach (var language in LanguageBranchRepository.Service.ListEnabled())
            {
                if (!(ContentRepository.Service.Get<IContentData>(e.ContentLink, language.Culture) is PageData pageData)) continue;

                var oldUrl = GetContentUrl(originalParent, language.Culture.Name, false);
                if (oldUrl == null)
                {
                    continue;
                }

                RedirectHelper.AddRedirects(pageData, oldUrl + pageData.URLSegment, language.Culture);
            }
        }

        private void SavingContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.AddAutomaticRedirects == false)
                return;

            var transition = (e as SaveContentEventArgs)?.Transition;
            if (transition.Value.CurrentStatus == VersionStatus.NotCreated) return;

            // create redirects only if page is unpublished
            // because child objects may have been already published so their URL changes
            if (ContentVersionRepository.Service.List(e.ContentLink).Any(p => p.Status == VersionStatus.Published || p.Status == VersionStatus.PreviouslyPublished)) return;

            var oldUrl = UrlResolver.Service.GetUrl(e.ContentLink);

            e.Items.Add(_oldUrlKey, oldUrl);
        }

        private void SavedContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.AddAutomaticRedirects == false)
                return;

            var oldUrl = e.Items[_oldUrlKey]?.ToString();
            if (oldUrl == null)
            {
                return;
            }

            var newUrl = UrlResolver.Service.GetUrl(e.ContentLink);

            if (newUrl != oldUrl)
            {
                var pageData = ContentRepository.Service.Get<IContentData>(e.ContentLink) as PageData;

                RedirectHelper.AddRedirects(pageData, oldUrl, GetCultureInfo(e));
            }

            e.Items.Remove(_oldUrlKey);
        }

        private void DeletedContentHandler(object sender, ContentEventArgs e)
        {
            RedirectHelper.DeleteRedirects(e.ContentLink, ((DeleteContentEventArgs)e).DeletedDescendents);
        }

        private static CultureInfo GetCultureInfo(ContentEventArgs e)
        {
            var localizable = e.Content as ILocalizable;
            return localizable?.Language;
        }
    }
}