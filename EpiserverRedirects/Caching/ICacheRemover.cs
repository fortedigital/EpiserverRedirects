namespace Forte.EpiserverRedirects.Caching
{
    public interface ICacheRemover
    {
        void RemoveByMasterKey(string masterKey);
        
        void Remove(string key);
    }
}
