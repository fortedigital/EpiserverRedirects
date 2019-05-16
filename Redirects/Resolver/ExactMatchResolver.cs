using System.Linq;
using System.Threading.Tasks;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.UrlPath;
using Forte.Redirects.Redirect;

namespace Forte.Redirects.Resolver
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
                .FirstOrDefault(r => r.OldPattern == oldPath.ToString() && r.RedirectRuleType == RedirectRuleType.ExactMatch);

            if (redirectRule == null)
                return null;

            return await Task.FromResult(new ExactMatchRedirect(redirectRule));
        }
    }
}