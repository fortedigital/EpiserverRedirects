using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EPiServer;
using EPiServer.Web;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Resolver
{
    public class RegexResolver : BaseRuleResolver, IRedirectRuleResolver
    {
        private readonly IRedirectRuleRepository _redirectRuleResolverRepository;

        public RegexResolver(IRedirectRuleRepository redirectRuleResolverRepository, IContentLoader contentLoader) : base(contentLoader)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            var currentSite = SiteDefinition.Current;
            var encodedOldPath = Uri.EscapeUriString(oldPath.ToString());
            var rule = _redirectRuleResolverRepository
                .GetAll()
                .Where(r => r.HostId == null || r.HostId == currentSite.Id)
                .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.Regex)
                .OrderBy(x => x.Priority)
                .AsEnumerable()
                .FirstOrDefault(r => Regex.IsMatch(encodedOldPath, r.OldPattern, RegexOptions.IgnoreCase));
            var result = ResolveRule(rule, r => new ExactMatchRedirect(r));
            return Task.FromResult(result);
        }
    }
}
