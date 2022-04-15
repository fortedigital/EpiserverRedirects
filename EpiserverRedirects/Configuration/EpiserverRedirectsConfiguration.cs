using Forte.EpiserverRedirects.Caching;
using Forte.EpiserverRedirects.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Forte.EpiserverRedirects.Configuration
{
    public class EpiserverRedirectsConfiguration
    {
        private readonly RedirectsOptions _options;

        public EpiserverRedirectsConfiguration(IServiceCollection services, RedirectsOptions redirectsOptions)
        {
            Services = services;
            _options = redirectsOptions;
        }

        public IServiceCollection Services { get; }

        public IServiceCollection AddCustomRepository<TRepository>() where TRepository : class, IRedirectRuleRepository
        {
            if (_options.Caching.AllRedirectsCacheEnabled)
            {
                Services.AddTransient<IRedirectRuleRepository>(
                    provider => new RedirectRuleCachedRepositoryDecorator(
                        ActivatorUtilities.CreateInstance<TRepository>(provider),
                        provider.GetService<ICache>()));

                Services.AddHostedService<CacheWarmupHostedService>();
            }
            else
            {
                Services.AddTransient<IRedirectRuleRepository, TRepository>();
            }

            return Services;
        }
    }
}
