using System.Linq;
using Forte.EpiserverRedirects.Caching;
using Forte.EpiserverRedirects.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

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
            RemovePreviousRegistrations();
            
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

        // Making sure that if multiple AddCustomRepository methods are called we don't have garbage in Services.
        private void RemovePreviousRegistrations()
        {
            Services.RemoveAll<IRedirectRuleRepository>();

            var cacheServiceDescriptor = Services.FirstOrDefault(
                descriptor => descriptor.ServiceType == typeof(IHostedService) &&
                              descriptor.ImplementationType == typeof(CacheWarmupHostedService));

            if (cacheServiceDescriptor != null)
            {
                Services.Remove(cacheServiceDescriptor);
            }
        }
    }
}
