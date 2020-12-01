namespace Forte.EpiserverRedirects.Configuration
{
    public interface ICacheConfiguration
    {
        bool IsUrlRedirectCacheEnabled { get; }
        bool IsAllRedirectsCacheEnabled { get; }
    }

    public class CacheConfiguration : ICacheConfiguration
    {
        public bool IsUrlRedirectCacheEnabled => Configuration.IsUrlRedirectCacheEnabled;

        public bool IsAllRedirectsCacheEnabled => Configuration.IsAllRedirectsCacheEnabled;     
    }
}