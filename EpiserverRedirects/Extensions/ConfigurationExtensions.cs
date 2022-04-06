using Microsoft.Extensions.Configuration;

namespace Forte.EpiserverRedirects.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetSection<T>(this IConfiguration configuration, string key)
            where T : new()
        {
            var t = new T();
            configuration.GetSection(key).Bind(t);
            return t;
        }
    }
}
