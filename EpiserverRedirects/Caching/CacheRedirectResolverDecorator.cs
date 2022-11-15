using System;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Redirect;
using Forte.EpiserverRedirects.Resolver;

namespace Forte.EpiserverRedirects.Caching
{
    public class CacheRedirectResolverDecorator : IRedirectRuleResolver
    {
        public const string CacheMasterKey = "Forte.EpiserverRedirects.IRedirect";
        private readonly IRedirectRuleResolver _redirectRuleResolver;
        private readonly ICache _cache;

        public CacheRedirectResolverDecorator(
            IRedirectRuleResolver redirectRuleResolver,
            ICache cache)
        {
            _redirectRuleResolver = redirectRuleResolver ?? throw new ArgumentNullException(nameof(redirectRuleResolver));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        public async Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            if (_cache.TryGet<IRedirect>(FormatCacheKey(oldPath), out var redirect))
            {
                return redirect;
            }

            redirect = await _redirectRuleResolver.ResolveRedirectRuleAsync(oldPath);
            _cache.Add(FormatCacheKey(oldPath), redirect, CacheMasterKey);
            return redirect;
        }

        private static string FormatCacheKey(UrlPath path) => $"{CacheMasterKey}_{path}";
    }
}
