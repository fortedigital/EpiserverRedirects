using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Repository.DynamicDataStore;
using Microsoft.Extensions.DependencyInjection;

namespace Forte.EpiserverRedirects.Extensions
{
    public static class EpiserverRedirectsConfigurationExtensions
    {
        public static IServiceCollection AddDynamicDataStoreRepository(this EpiserverRedirectsConfiguration configuration)
        {
            return configuration.AddCustomRepository<DynamicDataStoreRepository>();
        }
    }
}
