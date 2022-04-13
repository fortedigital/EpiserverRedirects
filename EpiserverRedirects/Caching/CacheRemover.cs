using EPiServer;

namespace Forte.EpiserverRedirects.Caching
{
    internal class CacheRemover : ICacheRemover
    {
        public virtual void RemoveByRegion(string region) => CacheManager.Remove(region);

        public virtual void Remove(string key)=> CacheManager.Remove(key);
    }
}
