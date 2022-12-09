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

        public IServiceCollection AddRulesRepository<TRepository>() where TRepository : class, IRedirectRuleRepository
        {
            if (Options.Caching.UrlRedirectCacheEnabled)
            {
                Services.AddTransient<TRepository>();
                Services.AddTransient<IRedirectRuleRepository>(
                    provider => new RedirectRuleCachedRepositoryDecorator(
                        provider.GetService<ICache>(),
                        provider.GetService<TRepository>()));
            }
            else
            {
                Services.AddTransient<IRedirectRuleRepository, TRepository>();
            }

            return Services;
        }
    }
}
