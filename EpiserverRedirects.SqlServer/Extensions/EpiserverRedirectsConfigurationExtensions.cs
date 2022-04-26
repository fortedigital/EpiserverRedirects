using System;
using EpiserverRedirects.EntityFramework.Extensions;
using EpiserverRedirects.SqlServer.Design;
using EpiserverRedirects.SqlServer.Services;
using Forte.EpiserverRedirects.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EpiserverRedirects.SqlServer.Extensions
{
    public static class EpiserverRedirectsConfigurationExtensions
    {
        public static IServiceCollection AddSqlServerRepository(
            this EpiserverRedirectsConfiguration configuration,
            string connectionString,
            Action<SqlServerDbContextOptionsBuilder> sqlServerConfigureAction = null)
        {
            configuration.Services.AddHostedService<MigrationService>();

            return configuration.AddEntityFrameworkRepository<SqlRedirectRulesDbContext>(
                dbContextBuilder => dbContextBuilder.UseSqlServer(
                    connectionString,
                    sqlServerBuilder => sqlServerConfigureAction?.Invoke(sqlServerBuilder)));
        }
    }
}
