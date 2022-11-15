using Forte.EpiserverRedirects.Caching;
using Forte.EpiserverRedirects.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Forte.EpiserverRedirects.Configuration
{
    public class EpiserverRedirectsRepositoryConfiguration
    {
        public EpiserverRedirectsRepositoryConfiguration(IServiceCollection services, RedirectsOptions options)
        {
            Services = services;
            Options = options;
        }

        public IServiceCollection Services { get; }
        public RedirectsOptions Options { get; }

        public IServiceCollection AddRepository<TRepository>()
            where TRepository : class, IRedirectRuleRepository
        {
            if (Options.Caching.AllRedirectsCacheEnabled)
            {
                Services.AddTransient<IRedirectRuleRepository>(
                    provider => new RedirectRuleCachedRepositoryDecorator(
                        ActivatorUtilities.CreateInstance<TRepository>(provider),
                        provider.GetService<ICache>()));

                // NO WARM UP
                // Services.AddHostedService<CacheWarmupHostedService>();
            }
            else
            {
                Services.AddTransient<IRedirectRuleRepository, TRepository>();
            }

            return Services;
        }
    }
}
