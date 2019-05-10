using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Redirect.Base;
using Forte.RedirectMiddleware.Redirect.Regex;
using Forte.RedirectMiddleware.Resolver.Base;

namespace Forte.RedirectMiddleware.Resolver.Regex
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
                .FirstOrDefault(r=>System.Text.RegularExpressions.Regex.IsMatch(oldPath.ToString(), r.OldPattern.ToString(), RegexOptions.IgnoreCase));

            if (redirectRule == null)
                return null;

            return await Task.FromResult(new RegexRedirect(redirectRule));
        }
    }
    
}