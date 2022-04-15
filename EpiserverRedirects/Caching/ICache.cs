namespace Forte.EpiserverRedirects.Caching
{
    public interface ICache
    {
        bool TryGet<T>(string key, out T cachedItem) where T : class;

        void Add<T>(string key, T item, string region = null) where T : class;

        void RemoveByRegion(string region);

        void Remove(string key);
    }
}
