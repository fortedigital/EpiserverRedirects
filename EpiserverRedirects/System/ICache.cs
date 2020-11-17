namespace Forte.EpiserverRedirects.System
{

    public interface ICacheRemover
    {
        void RemoveByRegion(string region);
        
        void Remove(string key);
    }
    
    public interface ICache<T> : ICacheRemover where T : class
    {
        bool TryGet(string key, out T resource);

        void Add(string key, T redirect, string region = null);
    }
}