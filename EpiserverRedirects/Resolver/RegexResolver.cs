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

        public Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            return Task.Run(() =>
            {
                var rule = _redirectRuleResolverRepository
                    .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.Regex)
                    .OrderBy(x=> x.Priority)
                    .AsEnumerable()
                    .FirstOrDefault(r => Regex.IsMatch(oldPath.ToString(), r.OldPattern.ToString(), RegexOptions.IgnoreCase));

                var redirectRule = (rule != null) ? new ExactMatchRedirect(rule) : new NullRedirectRule() as IRedirect;

                return redirectRule;
            });
        }
    }
}
