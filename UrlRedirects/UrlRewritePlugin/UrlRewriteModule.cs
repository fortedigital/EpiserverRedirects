using System.Collections.Generic;
using System.Globalization;
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

namespace UrlRedirects.UrlRewritePlugin
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class UrlRewriteModule : IInitializableModule
    {
        private const string _oldUrlKey = "OLD_URL";

        public void Initialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent += EventsMovedContent;
            events.PublishedContent += EventsPublishedContent;
            events.SavingContent += EventsSavingContent;
            events.SavedContent += EventsSavedContent;
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

            RedirectHelper.AddRedirects(pageData, oldUrl, GetCultureInfo(e));
        }

        public void Uninitialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent -= EventsMovedContent;
            events.PublishedContent -= EventsPublishedContent;
            events.SavingContent -= EventsSavingContent;
            events.SavedContent -= EventsSavedContent;
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

                RedirectHelper.AddRedirects(pageData, oldUrl, language.Culture);
            }
        }

        private static void EventsSavingContent(object sender, ContentEventArgs e)
        {
            var transition = (e as SaveContentEventArgs)?.Transition;
            if (transition.Value.CurrentStatus == VersionStatus.NotCreated) return;

            var cvr = ServiceLocator.Current.GetInstance<IContentVersionRepository>();
            if (cvr.List(e.ContentLink).Any(p => p.Status == VersionStatus.Published || p.Status == VersionStatus.PreviouslyPublished)) return;

            var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
            var oldUrl = urlHelper.ContentUrl(e.ContentLink);

            e.Items.Add(_oldUrlKey, oldUrl);
        }

        private static void EventsSavedContent(object sender, ContentEventArgs e)
        {
            var oldUrl = e.Items[_oldUrlKey]?.ToString();
            if (oldUrl != null)
            {
                var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
                var newUrl = urlHelper.ContentUrl(e.ContentLink);

                if (newUrl != oldUrl)
                {
                    var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
                    var pageData = contentRepository.Get<IContentData>(e.ContentLink) as PageData;

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