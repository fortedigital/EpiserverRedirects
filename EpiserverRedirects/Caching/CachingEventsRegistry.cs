using EPiServer.Data.Dynamic;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Caching
{
    public class CachingEventsRegistry
    {
        private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;
        private readonly ICacheRemover _cacheRemover;
        private readonly CacheOptions _options;

        public CachingEventsRegistry(DynamicDataStoreFactory dynamicDataStoreFactory, ICacheRemover cacheRemover, CacheOptions options)
        {
            _dynamicDataStoreFactory = dynamicDataStoreFactory;
            _cacheRemover = cacheRemover;
            _options = options;
        }

        public void RegisterEvents()
        {
            var redirectRuleDynamicDataStoreName = _dynamicDataStoreFactory.GetStoreNameForType(typeof(RedirectRule));
            DynamicDataStore.RegisterDeletedAllEventHandler(redirectRuleDynamicDataStoreName, HandleClearCache);
            DynamicDataStore.RegisterItemDeletedEventHandler(redirectRuleDynamicDataStoreName, HandleClearCache);
            DynamicDataStore.RegisterItemSavedEventHandler(redirectRuleDynamicDataStoreName, HandleClearCache);
        }

        private void HandleClearCache(object s, ItemEventArgs e)
        {
            if (_options.AllRedirectsCacheEnabled)
            {
                _cacheRemover.Remove(RedirectRuleCachedRepositoryDecorator.CacheKey);
            }

            if (_options.UrlRedirectCacheEnabled)
            {
                _cacheRemover.RemoveByMasterKey(CacheRedirectResolverDecorator.CacheMasterKey);
            }
        }
    }
}
