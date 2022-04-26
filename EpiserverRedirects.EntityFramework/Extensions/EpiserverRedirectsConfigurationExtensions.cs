using System;
using EpiserverRedirects.EntityFramework.Repository;
using Forte.EpiserverRedirects.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EpiserverRedirects.EntityFramework.Extensions
{
    public static class EpiserverRedirectsConfigurationExtensions
    {
        public static IServiceCollection AddEntityFrameworkRepository<TDbContext>(
            this EpiserverRedirectsConfiguration configuration,
            Action<DbContextOptionsBuilder> dbOptionsBuilder = null)
            where TDbContext : RedirectRulesDbContext
        {
            configuration.Services.AddDbContext<TDbContext>(dbOptionsBuilder);

            return configuration.AddCustomRepository<EntityFrameworkRepository<TDbContext>>();
        }
    }
}
