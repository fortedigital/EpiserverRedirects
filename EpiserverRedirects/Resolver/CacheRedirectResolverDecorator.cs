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
        private readonly ICache<IRedirect> _cache;
        public const string CacheRegionKey = "Forte.EpiserverRedirects";

        public CacheRedirectResolverDecorator(
            IRedirectRuleResolver redirectRuleResolver,
            ICache<IRedirect> cache)
        {
            _redirectRuleResolver = redirectRuleResolver ?? throw new ArgumentNullException(nameof(redirectRuleResolver));
            _cache = cache?? throw  new ArgumentNullException(nameof(cache));
        }


        public async Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            if (_cache.TryGet(FormatCacheKey(oldPath), out var redirect))
            {
                return redirect;
            }

            redirect = await _redirectRuleResolver.ResolveRedirectRuleAsync(oldPath);
            _cache.Add(FormatCacheKey(oldPath), redirect, CacheRegionKey);
            return redirect;
        }
        
        private static string FormatCacheKey(UrlPath path) => $"{CacheRegionKey}_{path}";
    }
}