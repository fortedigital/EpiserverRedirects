using System.Linq;
using EPiServer;
using EPiServer.Framework.Cache;

namespace Forte.EpiserverRedirects.Caching
{
    internal class CacheRemover : ICacheRemover
    {
        public virtual void RemoveByRegion(string region) => CacheManager.Remove(region);

        public virtual void Remove(string key)=> CacheManager.Remove(key);
    }

    internal class Cache : CacheRemover, ICache
    {
        public bool TryGet<T>(string key, out T cachedItem) where T : class
        {
            cachedItem = CacheManager.Get(key) as T;
            return cachedItem != null;
        }

        public void Add<T>(string key, T cachedItem, string region) where T : class => CacheManager.Insert(key, cachedItem, CreateCachePolicy(region));

        private static CacheEvictionPolicy CreateCachePolicy(string region)
        {
            return string.IsNullOrWhiteSpace(region) ? CacheEvictionPolicy.Empty : new CacheEvictionPolicy(Enumerable.Empty<string>(), new[] {region});
        }
    }
}
