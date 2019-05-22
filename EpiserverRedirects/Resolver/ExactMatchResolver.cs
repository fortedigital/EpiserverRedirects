using System.Linq;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.Resolver
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