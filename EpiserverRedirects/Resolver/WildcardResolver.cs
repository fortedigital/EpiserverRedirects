using System.Linq;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.Resolver
{
    public class WildcardResolver : IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public WildcardResolver(IQueryable<RedirectRule> redirectRuleResolverRepository)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public async Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
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