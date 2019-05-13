using System.Linq;
using System.Threading.Tasks;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Redirect.Base;
using Forte.RedirectMiddleware.Redirect.Wildcard;
using Forte.RedirectMiddleware.Resolver.Base;

namespace Forte.RedirectMiddleware.Resolver.Wildcard
{
    public class WildcardResolver : IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public WildcardResolver(IQueryable<RedirectRule> redirectRuleResolverRepository)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public async Task<IRedirect> ResolveRedirectRule(UrlPath oldPath)
        {
            var redirectRule = _redirectRuleResolverRepository
                .Where(r=>r.RedirectRuleType == RedirectRuleType.Wildcard)
                .AsEnumerable()
                .FirstOrDefault();
            
            if (redirectRule == null)
                return null;

            return await Task.FromResult(new WildcardRedirect(redirectRule));
        }
        
    }
}