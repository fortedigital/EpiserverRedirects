using System.Threading;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.Repository;
using Microsoft.Extensions.Hosting;

namespace Forte.EpiserverRedirects.Caching
{
    public class CacheWarmupHostedService : IHostedService
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;

        public CacheWarmupHostedService(IRedirectRuleRepository redirectRuleRepository)
        {
            _redirectRuleRepository = redirectRuleRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _redirectRuleRepository.GetAll();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
