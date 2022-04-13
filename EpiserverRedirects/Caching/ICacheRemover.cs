namespace Forte.EpiserverRedirects.Caching
{
    public interface ICacheRemover
    {
        void RemoveByRegion(string region);
        
        void Remove(string key);
    }
}
