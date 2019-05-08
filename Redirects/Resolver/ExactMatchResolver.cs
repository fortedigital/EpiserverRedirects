using System.Linq;
using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Repository;

namespace Forte.RedirectMiddleware.Resolver
{
//    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleResolver))]
    public class ExactMatchResolver : IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public ExactMatchResolver(IQueryable<RedirectRule> redirectRuleResolverRepository)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public async Task<RedirectRule> ResolveRedirectRule(UrlPath oldPath)
        {
            var redirectRule = _redirectRuleResolverRepository
                .FirstOrDefault(r => r.OldPath == oldPath);

            return await Task.FromResult(redirectRule);
        }
        
    }
}