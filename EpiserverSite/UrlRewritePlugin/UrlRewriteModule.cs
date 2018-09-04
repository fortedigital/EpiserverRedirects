using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
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
            var newUrl = urlHelper.ContentUrl(e.ContentLink);
            var cvr = ServiceLocator.Current.GetInstance<IContentVersionRepository>();
            var lastVersion = cvr
                .List(e.ContentLink)
                .Where(p => p.Status == VersionStatus.PreviouslyPublished)
                .OrderByDescending(p => p.Saved)
                .FirstOrDefault();

            if (lastVersion == null) return;
            var oldUrl = urlHelper.ContentUrl(lastVersion.ContentLink);
            if (newUrl.Equals(oldUrl)) return;

            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var pageData = contentRepository.Get<IContentData>(lastVersion.ContentLink) as PageData;
            
            RedirectHelper.AddRedirects(pageData, oldUrl, newUrl);
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
            var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
            var newUrl = urlHelper.ContentUrl(e.ContentLink);

            var originalParent = (e as MoveContentEventArgs)?.OriginalParent;
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

            if (!(contentRepository.Get<IContentData>(e.ContentLink) is PageData pageData)) return;
            
            var virtualPathArguments = new VirtualPathArguments
            {
                ValidateTemplate = false
            };

            var oldUrl = 
                UrlResolver.Current.GetUrl(originalParent, 
                    ContentLanguage.PreferredCulture.Name, virtualPathArguments)
                + pageData.URLSegment;

            RedirectHelper.AddRedirects(pageData, oldUrl, newUrl);
        }
    }
}