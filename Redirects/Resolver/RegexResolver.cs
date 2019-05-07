using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model.RedirectResult;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;

namespace Forte.RedirectMiddleware.Resolver
{
    public class RegexResolver : IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public RegexResolver(IQueryable<RedirectRule> redirectRuleResolverRepository)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public async Task<RedirectRule> ResolveRedirectRule(UrlPath oldPath)
        {
            var redirectRule = _redirectRuleResolverRepository
                .Where(r=>r.RedirectRuleType == RedirectRuleType.Regex)
                .AsEnumerable()
                .FirstOrDefault(r=>Regex.IsMatch(oldPath.ToString(), r.OldPattern.ToString(), RegexOptions.IgnoreCase));

            if (redirectRule == null)
                return null;

            redirectRule.NewPattern = Regex.Replace(oldPath.ToString(), redirectRule.OldPath.ToString(),
                redirectRule.NewPattern, RegexOptions.IgnoreCase);

            return await Task.FromResult(redirectRule);
        }
        
    }
}