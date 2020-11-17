using System;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Redirect;
using Forte.EpiserverRedirects.System;

namespace Forte.EpiserverRedirects.Resolver
{
    public class CacheRedirectResolverDecorator : IRedirectRuleResolver
    {
        private readonly IRedirectRuleResolver _redirectRuleResolver;
        private readonly IRuleRedirectCache _ruleRedirectCache;

        public CacheRedirectResolverDecorator(
            IRedirectRuleResolver redirectRuleResolver,
            IRuleRedirectCache ruleRedirectCache)
        {
            _redirectRuleResolver = redirectRuleResolver ?? throw new ArgumentNullException(nameof(redirectRuleResolver));
            _ruleRedirectCache = ruleRedirectCache?? throw  new ArgumentNullException(nameof(ruleRedirectCache));
        }


        public async Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            if (_ruleRedirectCache.TryGet(oldPath, out var redirect))
            {
                return redirect;
            }

            redirect = await _redirectRuleResolver.ResolveRedirectRuleAsync(oldPath);
            _ruleRedirectCache.Add(oldPath, redirect);
            return redirect;
        }
    }
}