using System.Globalization;
using System.Linq;
using EPiServer;
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
            events.MovedContent += EventsMovedContent;
            events.PublishedContent += EventsPublishedContent;
            events.SavingContent += EventsSavingContent;
            events.SavedContent += EventsSavedContent;
        }

        private void EventsPublishedContent(object sender, ContentEventArgs e)
        {
            if (Configuration.AddAutomaticRedirects == false)
                return;
            
            var lastVersion = ContentVersionRepository.Service
                .List(e.ContentLink)
                .Where(p => p.Status == VersionStatus.PreviouslyPublished)
                .OrderByDescending(p => p.Saved)
                .FirstOrDefault();
            
            if (lastVersion == null) return;

            var oldUrl = GetContentUrl(lastVersion.ContentLink, lastVersion.LanguageBranch);
            if (oldUrl == null)
            {
                return;
            }
            
            var pageData = ContentRepository.Service.Get<IContentData>(lastVersion.ContentLink) as PageData;

            RedirectHelper.AddRedirects(pageData, oldUrl, GetCultureInfo(e));
        }

        private string GetContentUrl(ContentReference contentReference, string language, bool validateTemplate = true)
        {
            var arguments = new VirtualPathArguments {ValidateTemplate = validateTemplate};
            return UrlResolver.Service.GetUrl(contentReference, language, arguments);
        }

        public void Uninitialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent -= EventsMovedContent;
            events.PublishedContent -= EventsPublishedContent;
            events.SavingContent -= EventsSavingContent;
            events.SavedContent -= EventsSavedContent;
        }

        private void EventsMovedContent(object sender, ContentEventArgs e)
        {
            if (Configuration.AddAutomaticRedirects == false)
                return;
            
            if (!(e.Content is IChangeTrackable)) return;

            var originalParent = (e as MoveContentEventArgs)?.OriginalParent;
            
            foreach (var language in LanguageBranchRepository.Service.ListEnabled())
            {
                if (!(ContentRepository.Service.Get<IContentData>(e.ContentLink, language.Culture) is PageData pageData)) return;

                var oldUrl = GetContentUrl(originalParent, language.Culture.Name);
                if (oldUrl == null)
                {
                    continue;                    
                }

                RedirectHelper.AddRedirects(pageData, oldUrl + pageData.URLSegment, language.Culture);
            }
        }

        private void EventsSavingContent(object sender, ContentEventArgs e)
        {
            if (Configuration.AddAutomaticRedirects == false)
                return;
            
            var transition = (e as SaveContentEventArgs)?.Transition;
            if (transition.Value.CurrentStatus == VersionStatus.NotCreated) return;

            if (ContentVersionRepository.Service.List(e.ContentLink).Any(p => p.Status == VersionStatus.Published || p.Status == VersionStatus.PreviouslyPublished)) return;

            var oldUrl = UrlResolver.Service.GetUrl(e.ContentLink);

            e.Items.Add(_oldUrlKey, oldUrl);
        }

        private void EventsSavedContent(object sender, ContentEventArgs e)
        {
            if (Configuration.AddAutomaticRedirects == false)
                return;
            
            var oldUrl = e.Items[_oldUrlKey]?.ToString();
            if (oldUrl != null)
            {
                var newUrl = UrlResolver.Service.GetUrl(e.ContentLink);

                if(newUrl != oldUrl)
                {
                    var pageData = ContentRepository.Service.Get<IContentData>(e.ContentLink) as PageData;

                    RedirectHelper.AddRedirects(pageData, oldUrl, GetCultureInfo(e));
                }

                e.Items.Remove(_oldUrlKey);
            }
        }

        private static CultureInfo GetCultureInfo(ContentEventArgs e)
        {
            var localizable = e.Content as ILocalizable;
            return localizable?.Language;
        }
    }
}