using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;

namespace EpiserverSite.UrlRewritePlugin
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class UrlRewriteModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent += EventsMovedContent;
            events.PublishedContent += EventsPublishedContent;
        }

        private static void EventsPublishedContent(object sender, ContentEventArgs e)
        {
            var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
            var cvr = ServiceLocator.Current.GetInstance<IContentVersionRepository>();
            var lastVersion = cvr
                .List(e.ContentLink)
                .Where(p => p.Status == VersionStatus.PreviouslyPublished)
                .OrderByDescending(p => p.Saved)
                .FirstOrDefault();

            if (lastVersion == null) return;
            var oldUrl = urlHelper.ContentUrl(lastVersion.ContentLink);

            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var pageData = contentRepository.Get<IContentData>(lastVersion.ContentLink) as PageData;
            
            RedirectHelper.AddRedirects(pageData, oldUrl);
        }

        public void Uninitialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent -= EventsMovedContent;
            events.PublishedContent -= EventsPublishedContent;
        }

        private static void EventsMovedContent(object sender, ContentEventArgs e)
        {
            if (!(e.Content is IChangeTrackable)) return;

            var originalParent = (e as MoveContentEventArgs)?.OriginalParent;
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var languageBranchRepository = ServiceLocator.Current.GetInstance<ILanguageBranchRepository>();
            var virtualPathArguments = new VirtualPathArguments
            {
                ValidateTemplate = false
            };

            foreach (var language in languageBranchRepository.ListEnabled())
            {
                if (!(contentRepository.Get<IContentData>(e.ContentLink, language.Culture) is PageData pageData)) return;

                var oldUrl =
                    UrlResolver.Current.GetUrl(originalParent,
                        language.Culture.Name, virtualPathArguments)
                    + pageData.URLSegment;

                RedirectHelper.AddRedirects(pageData, oldUrl);
            }
        }
    }
}