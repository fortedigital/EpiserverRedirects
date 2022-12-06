using System.Linq;
using EPiServer;
using EPiServer.Framework.Cache;

namespace Forte.EpiserverRedirects.Caching
{
    internal class Cache : ICache
    {
        public bool TryGet<T>(string key, out T item) where T : class
        {
            item = CacheManager.Get(key) as T;

            return item != null;
        }

        public void Add<T>(string key, T item, string masterKey) where T : class => CacheManager.Insert(key, item, CreateCachePolicy(masterKey));

        public void RemoveByMasterKey(string masterKey) => CacheManager.Remove(masterKey);

        public void Remove(string key) => CacheManager.Remove(key);

        private static CacheEvictionPolicy CreateCachePolicy(string masterKey)
        {
            return string.IsNullOrWhiteSpace(masterKey)
                ? CacheEvictionPolicy.Empty
                : new CacheEvictionPolicy(Enumerable.Empty<string>(), new[] {masterKey});
        }
    }
}
