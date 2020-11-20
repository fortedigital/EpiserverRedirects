using System.Linq;
using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.Data.Dynamic;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.System
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class SystemRedirectsEventsModule : IInitializableModule
    {
        private const string OldUrlKey = "OLD_URL";

        private Injected<UrlResolver> UrlResolver { get; set; }
        private Injected<IContentVersionRepository> ContentVersionRepository { get; set; }
        private Injected<IContentRepository> ContentRepository { get; set; }
        private Injected<ILanguageBranchRepository> LanguageBranchRepository { get; set; }
        private Injected<ICacheRemover> CacheRemover { get; set; }
        
        private  Injected<ICacheConfiguration> CacheConfiguration { get; set; }

        public void Initialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent += MovedContentHandler;
            events.PublishedContent += PublishedContentHandler;
            events.SavingContent += SavingContentHandler;
            events.SavedContent += SavedContentHandler;
            events.DeletedContent += DeletedContentHandler;

            RegisterRedirectDynamicStoreCacheHandlers();
        }

        public void Uninitialize(InitializationEngine context)
        {
            var events = context.Locate.ContentEvents();
            events.MovedContent -= MovedContentHandler;
            events.PublishedContent -= PublishedContentHandler;
            events.SavingContent -= SavingContentHandler;
            events.SavedContent -= SavedContentHandler;
            events.DeletedContent -= DeletedContentHandler;

            UnregisterRedirectDynamicStoreCacheHandlers();
        }

        private void PublishedContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.Configuration.AddAutomaticRedirects == false)
                return;

            var lastVersion = ContentVersionRepository
                .Service
                .List(e.ContentLink)
                .Where(p => p.Status == VersionStatus.PreviouslyPublished)
                .OrderByDescending(p => p.Saved)
                .FirstOrDefault();

            if (lastVersion == null)
                return;

            var oldUrl = GetContentUrl(lastVersion.ContentLink, lastVersion.LanguageBranch);

            if (oldUrl == null)
                return;

            var newUrl = GetContentUrl(e.ContentLink, e.Content.LanguageBranch());
            if (oldUrl == newUrl)
                return;

            var lastVersionPageData = ContentRepository.Service.Get<IContentData>(lastVersion.ContentLink) as PageData;
            if (lastVersionPageData == null)
                return;

            SystemRedirectsActions.AddRedirects(lastVersionPageData, oldUrl, SystemRedirectsHelper.GetCultureInfo(e), SystemRedirectReason.PublishedContent);
        }

        private void MovedContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.Configuration.AddAutomaticRedirects == false)
                return;

            if (!(e.Content is IChangeTrackable))
                return;

            var originalParent = (e as MoveContentEventArgs)?.OriginalParent;

            if (originalParent == ContentReference.WasteBasket)
            {
                // do not create when restoring, cause not need to do redirects from waste basket.
                // however, DO redirect when moving to waste basket, because restore may be to another place 
                return;
            }

            foreach (var language in LanguageBranchRepository.Service.ListEnabled())
            {
                if (!(ContentRepository.Service.Get<IContentData>(e.ContentLink, language.Culture) is PageData pageData))
                    continue;

                var oldUrl = GetContentUrl(originalParent, language.Culture.Name, false);
                if (oldUrl == null)
                    continue;

                SystemRedirectsActions.AddRedirects(pageData, oldUrl + pageData.URLSegment, language.Culture, SystemRedirectReason.MovedContent);
            }
        }

        private void SavingContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.Configuration.AddAutomaticRedirects == false)
                return;

            var transition = (e as SaveContentEventArgs)?.Transition;
            if (transition.Value.CurrentStatus == VersionStatus.NotCreated) return;

            // create redirects only if page is unpublished
            // because child objects may have been already published so their URL changes
            if (ContentVersionRepository
                .Service
                .List(e.ContentLink)
                .Any(p => p.Status == VersionStatus.Published || p.Status == VersionStatus.PreviouslyPublished))
                return;

            var oldUrl = UrlResolver.Service.GetUrl(e.ContentLink, null);

            e.Items.Add(OldUrlKey, oldUrl);
        }

        private void SavedContentHandler(object sender, ContentEventArgs e)
        {
            if (Configuration.Configuration.AddAutomaticRedirects == false)
                return;

            var oldUrl = e.Items[OldUrlKey]?.ToString();
            if (oldUrl == null)
                return;

            var newUrl = UrlResolver.Service.GetUrl(e.ContentLink, null);

            if (newUrl != oldUrl)
            {
                var pageData = ContentRepository.Service.Get<IContentData>(e.ContentLink) as PageData;
                SystemRedirectsActions.AddRedirects(pageData, oldUrl, SystemRedirectsHelper.GetCultureInfo(e), SystemRedirectReason.SavedContent);
            }

            e.Items.Remove(OldUrlKey);
        }

        private static void DeletedContentHandler(object sender, ContentEventArgs e)
        {
            SystemRedirectsActions.DeleteRedirects(e.ContentLink, ((DeleteContentEventArgs) e).DeletedDescendents);
        }

        private string GetContentUrl(ContentReference contentReference, string language, bool validateTemplate = true)
        {
            var arguments = new VirtualPathArguments {ValidateTemplate = validateTemplate};
            return UrlResolver.Service.GetVirtualPath(contentReference, language, arguments)?.VirtualPath;
        }

        private void RegisterRedirectDynamicStoreCacheHandlers()
        {
            if (!CacheConfiguration.Service.IsAllRedirectsCacheEnabled && !CacheConfiguration.Service.IsUrlRedirectCacheEnabled)
            {
                return;
            }
            
            var redirectRuleDynamicDataStoreName = DynamicDataStoreFactory.Instance.GetStoreNameForType(typeof(RedirectRule));
            DynamicDataStore.RegisterDeletedAllEventHandler(redirectRuleDynamicDataStoreName, HandleClearCache);
            DynamicDataStore.RegisterItemDeletedEventHandler(redirectRuleDynamicDataStoreName, HandleClearCache);
            DynamicDataStore.RegisterItemSavedEventHandler(redirectRuleDynamicDataStoreName, HandleClearCache);
        }

        private void UnregisterRedirectDynamicStoreCacheHandlers()
        {
            if (!CacheConfiguration.Service.IsAllRedirectsCacheEnabled && !CacheConfiguration.Service.IsUrlRedirectCacheEnabled)
            {
                return;
            }
            
            var redirectRuleDynamicDataStoreName = DynamicDataStoreFactory.Instance.GetStoreNameForType(typeof(RedirectRule));
            DynamicDataStore.UnregisterDeletedAllEventHandler(redirectRuleDynamicDataStoreName, HandleClearCache);
            DynamicDataStore.UnregisterItemDeletedEventHandler(redirectRuleDynamicDataStoreName, HandleClearCache);
            DynamicDataStore.UnregisterItemSavedEventHandler(redirectRuleDynamicDataStoreName, HandleClearCache);
        }

        private void HandleClearCache(object s, ItemEventArgs e)
        {
            if (CacheConfiguration.Service.IsAllRedirectsCacheEnabled)
            {
                CacheRemover.Service.Remove(RedirectRuleCachedRepositoryDecorator.CacheKey);
            }

            if (CacheConfiguration.Service.IsUrlRedirectCacheEnabled)
            {
                CacheRemover.Service.RemoveByRegion(CacheRedirectResolverDecorator.CacheRegionKey);
            }
        }
    }
}