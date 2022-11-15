using EPiServer;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Redirect;
using Forte.EpiserverRedirects.Repository;
using System;
using System.Threading.Tasks;


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
            var encodedOldPath = Uri.EscapeUriString(oldPath.ToString());
            var rule = _redirectRuleResolverRepository.FindRegexMatch(encodedOldPath);
            var result = ResolveRule(rule, r => new ExactMatchRedirect(r));
            return Task.FromResult(result);
        }
    }
}
