using System;
using System.Threading;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.SqlServer.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Forte.EpiserverRedirects.SqlServer.Services
{
    internal class MigrationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MigrationService> _logger;

        public MigrationService(IServiceProvider serviceProvider, ILogger<MigrationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<SqlRedirectRulesDbContext>();
            if (dbContext == null)
            {
                return;
            }

            _logger.LogTrace("Running automatic migrations...");

            await dbContext.Database.MigrateAsync(cancellationToken);

            _logger.LogTrace("All migrations applied");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
