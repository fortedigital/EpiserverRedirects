using System;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


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
            configuration.Services.AddSingleton<IRedirectRuleMapper, RedirectRuleMapper>();
            return configuration.AddRepository<RedirectRulesRepository<TDbContext>>();
        }
    }
}
