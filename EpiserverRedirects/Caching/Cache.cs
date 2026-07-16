using System.Linq;
using EPiServer.Framework.Cache;

namespace Forte.EpiserverRedirects.Caching
{
    internal class Cache : ICache
    {
        private readonly ISynchronizedObjectInstanceCache _cache;

        public Cache(ISynchronizedObjectInstanceCache cache)
        {
            _cache = cache;
        }

        public bool TryGet<T>(string key, out T item) where T : class
        {
            item = _cache.Get(key) as T;

            return item != null;
        }

        public void Add<T>(string key, T item, string masterKey) where T : class => _cache.Insert(key, item, CreateCachePolicy(masterKey));

        public void RemoveByMasterKey(string masterKey) => _cache.Remove(masterKey);

        public void Remove(string key) => _cache.Remove(key);

        private static CacheEvictionPolicy CreateCachePolicy(string masterKey)
        {
            return string.IsNullOrWhiteSpace(masterKey)
                ? CacheEvictionPolicy.Empty
                : new CacheEvictionPolicy(Enumerable.Empty<string>(), new[] {masterKey});
        }
    }
}
