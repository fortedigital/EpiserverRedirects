using System.Linq;
using System.Threading.Tasks;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.UrlPath;
using Forte.Redirects.Redirect;

namespace Forte.Redirects.Resolver
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