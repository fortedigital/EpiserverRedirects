using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Microsoft.Extensions.DependencyInjection;


namespace Forte.EpiserverRedirects.DynamicData.Extensions
{
    public static class EpiserverRedirectsRepositoryConfigurationExtensions
    {
        public static IServiceCollection AddDynamicDataRuleRepository(
            this EpiserverRedirectsRepositoryConfiguration configuration)
        {
            configuration.Services.AddTransient<IDynamicDataStore<RedirectRule>, DynamicDataStore<RedirectRule>>();
            configuration.Services.AddSingleton<IRedirectRuleMapper<RedirectRule>, DynamicDataRedirectRuleMapper>();
            return configuration.AddRulesRepository<DynamicDataRepository>();
        }
    }
}
