using System.Configuration;

namespace Forte.EpiserverRedirects.Configuration
{
    public static class Configuration
    {
        public static bool PreserveQueryString = false;
        public static bool AddAutomaticRedirects = true;
        public static int SystemRedirectRulePriority = 100;
        public static int DefaultRedirectRulePriority = 100;
        public static bool IsUrlRedirectCacheEnabled => IsAppSettingsConfigTrue("Forte.EpiserverRedirects:UrlRedirectCacheEnabled");

        public static bool IsAllRedirectsCacheEnabled => IsAppSettingsConfigTrue("Forte.EpiserverRedirects:AllRedirectCacheEnabled"); 
        
        private static bool IsAppSettingsConfigTrue(string appSettingsKey, bool defaultValue = false)
        {
            var config = ConfigurationManager.AppSettings[appSettingsKey];
            if (!string.IsNullOrWhiteSpace(config) && bool.TryParse(config, out var configResult))
            {
                return configResult;
            }

            return defaultValue;
        }
        
    }
}
