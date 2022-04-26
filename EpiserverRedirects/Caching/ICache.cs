namespace Forte.EpiserverRedirects.Caching
{
    public interface ICache : ICacheRemover
    {
        bool TryGet<T>(string key, out T item) where T : class;

        void Add<T>(string key, T item, string masterKey = null) where T : class;
    }
}
