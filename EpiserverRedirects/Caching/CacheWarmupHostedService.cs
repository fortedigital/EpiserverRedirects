using System;
using System.Threading;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.Repository;
using Microsoft.Extensions.Hosting;


namespace Forte.EpiserverRedirects.Caching
{
    [Obsolete("No need to use, as we do not store all rules in cache. We cache only matches.")]
    public class CacheWarmupHostedService : IHostedService
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;

        public CacheWarmupHostedService(IRedirectRuleRepository redirectRuleRepository)
        {
            _redirectRuleRepository = redirectRuleRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TURNED OFF - no warming up needed
            // _redirectRuleRepository.GetAll();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
