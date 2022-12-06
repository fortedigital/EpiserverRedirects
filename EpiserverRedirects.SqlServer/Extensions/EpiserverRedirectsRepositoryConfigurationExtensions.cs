using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.EntityFramework.Extensions;
using Forte.EpiserverRedirects.SqlServer.Design;
using Forte.EpiserverRedirects.SqlServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Forte.EpiserverRedirects.SqlServer.Extensions
{
    public static class EpiserverRedirectsRepositoryConfigurationExtensions
    {
        public static IServiceCollection AddSqlServerRuleRepository(
            this EpiserverRedirectsRepositoryConfiguration configuration,
            string connectionString,
            Action<SqlServerDbContextOptionsBuilder> sqlServerConfigureAction = null)
        {
            configuration.Services.AddHostedService<MigrationService>();
            return configuration.AddEntityFrameworkContext<SqlRedirectRulesDbContext>(
                dbContextBuilder => dbContextBuilder.UseSqlServer(
                    connectionString,
                    sqlServerBuilder => sqlServerConfigureAction?.Invoke(sqlServerBuilder)));
        }
    }
}
