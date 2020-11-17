using System.Linq;
using EPiServer;
using EPiServer.Framework.Cache;

namespace Forte.EpiserverRedirects.System
{
    internal class CacheRemover : ICacheRemover
    {
        public virtual void RemoveByRegion(string region) => CacheManager.Remove(region);
        
        public virtual void Remove(string key)=> CacheManager.Remove(key);
    }
    
    internal class Cache<T> :CacheRemover,  ICache<T> where T : class
    {
        public bool TryGet(string key, out T cachedItem)
        {
            cachedItem = CacheManager.Get(key) as T;
            return cachedItem != null;
        }

        public void Add(string key, T cachedItem, string region) => CacheManager.Insert(key, cachedItem, CreateCachePolicy(region));

        private static CacheEvictionPolicy CreateCachePolicy(string region)
        {
            return string.IsNullOrWhiteSpace(region) ? CacheEvictionPolicy.Empty : new CacheEvictionPolicy(Enumerable.Empty<string>(), new[] {region});
        }
    }
}
