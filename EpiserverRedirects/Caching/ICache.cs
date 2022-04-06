namespace Forte.EpiserverRedirects.Caching
{
    public interface ICacheRemover
    {
        void RemoveByRegion(string region);
        
        void Remove(string key);
    }
    
    public interface ICache : ICacheRemover
    {
        bool TryGet<T>(string key, out T resource) where T : class;

        void Add<T>(string key, T redirect, string region = null) where T : class;
    }
}
