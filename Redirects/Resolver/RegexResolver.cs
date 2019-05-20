using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Forte.Redirects.Model;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Redirect;

namespace Forte.Redirects.Resolver
{
    public class RegexResolver : IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public RegexResolver(IQueryable<RedirectRule> redirectRuleResolverRepository)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public async Task<IRedirect> ResolveRedirectRule(UrlPath oldPath)
        {
            var redirectRule = _redirectRuleResolverRepository
                .Where(r=>r.RedirectRuleType == RedirectRuleType.Regex)
                .AsEnumerable()
                .FirstOrDefault(r=>Regex.IsMatch(oldPath.ToString(), r.OldPattern.ToString(), RegexOptions.IgnoreCase));

            if (redirectRule == null)
                return null;

            return await Task.FromResult(new RegexRedirect(redirectRule));
        }
    }
    
}