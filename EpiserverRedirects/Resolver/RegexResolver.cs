using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.Resolver
{
    public class RegexResolver : IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public RegexResolver(IQueryable<RedirectRule> redirectRuleResolverRepository)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public async Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
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