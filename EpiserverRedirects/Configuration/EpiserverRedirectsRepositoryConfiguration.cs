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
            return Services.AddTransient<IRedirectRuleRepository, TRepository>();
        }
    }
}
