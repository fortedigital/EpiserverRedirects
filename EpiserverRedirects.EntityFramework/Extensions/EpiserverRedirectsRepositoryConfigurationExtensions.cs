using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.EntityFramework.Repository;
using Forte.EpiserverRedirects.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Forte.EpiserverRedirects.EntityFramework.Extensions
{
    public static class EpiserverRedirectsRepositoryConfigurationExtensions
    {
        public static IServiceCollection AddEntityFrameworkContext<TDbContext>(
            this EpiserverRedirectsRepositoryConfiguration configuration,
            Action<DbContextOptionsBuilder> dbOptionsBuilder = null)
            where TDbContext : RedirectRulesDbContext
        {
            configuration.Services.AddDbContext<IRedirectRulesDbContext, TDbContext>(dbOptionsBuilder);
            configuration.Services.AddSingleton<IRedirectRuleMapper<RedirectRuleEntity>, RedirectRuleMapper>();
            return configuration.AddRulesRepository<RedirectRulesRepository>();
        }
    }
}
