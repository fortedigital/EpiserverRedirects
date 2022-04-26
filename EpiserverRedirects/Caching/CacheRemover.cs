using EPiServer;

namespace Forte.EpiserverRedirects.Caching
{
    internal class CacheRemover : ICacheRemover
    {
        public virtual void RemoveByMasterKey(string masterKey) => CacheManager.Remove(masterKey);

        public virtual void Remove(string key) => CacheManager.Remove(key);
    }
}
