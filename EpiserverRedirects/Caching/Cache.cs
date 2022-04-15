using System.Linq;
using EPiServer;
using EPiServer.Framework.Cache;

namespace Forte.EpiserverRedirects.Caching
{
    internal class Cache : ICache
    {
        public bool TryGet<T>(string key, out T cachedItem) where T : class
        {
            cachedItem = CacheManager.Get(key) as T;

            return cachedItem != null;
        }

        public void Add<T>(string key, T item, string region) where T : class => CacheManager.Insert(key, item, CreateCachePolicy(region));

        public void RemoveByRegion(string region) => CacheManager.Remove(region);

        public void Remove(string key) => CacheManager.Remove(key);

        private static CacheEvictionPolicy CreateCachePolicy(string region)
        {
            return string.IsNullOrWhiteSpace(region) ? CacheEvictionPolicy.Empty : new CacheEvictionPolicy(Enumerable.Empty<string>(), new[] {region});
        }
    }
}
