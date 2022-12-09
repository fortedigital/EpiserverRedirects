using Forte.EpiserverRedirects.Caching;

namespace Forte.EpiserverRedirects.Configuration
{
    public class RedirectsOptions
    {
        public CacheOptions Caching { get; set; } = new CacheOptions();

        public bool PreserveQueryString { get; set; } = false;

        public bool AddAutomaticRedirects { get; set; } = true;

        public int SystemRedirectRulePriority { get; set; } = 100;

        public int DefaultRedirectRulePriority { get; set; } = 100;
    }
}
