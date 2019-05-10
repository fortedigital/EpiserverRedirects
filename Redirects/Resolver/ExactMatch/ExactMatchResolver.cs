using System.Linq;
using System.Threading.Tasks;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Redirect.Base;
using Forte.RedirectMiddleware.Redirect.ExactMatch;
using Forte.RedirectMiddleware.Resolver.Base;

namespace Forte.RedirectMiddleware.Resolver.ExactMatch
{
    public class ExactMatchResolver : IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public ExactMatchResolver(IQueryable<RedirectRule> redirectRuleResolverRepository)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public async Task<IRedirect> ResolveRedirectRule(UrlPath oldPath)
        {
            var redirectRule = _redirectRuleResolverRepository
                .FirstOrDefault(r => r.OldPath == oldPath && r.RedirectRuleType == RedirectRuleType.ExactMatch);

            return await Task.FromResult(new ExactMatchRedirect(redirectRule));
        }
        
    }
}