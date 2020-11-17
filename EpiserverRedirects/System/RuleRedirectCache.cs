using System.Linq;
using EPiServer;
using EPiServer.Framework.Cache;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.System
{
    internal class RuleRedirectCache : IRuleRedirectCache
    {
        private const string MasterPolicyKey = "Forte.EpiserverRedirects";

        public void RemoveAll() => CacheManager.Remove(MasterPolicyKey);

        public bool TryGet(UrlPath urlPath, out IRedirect resource)
        {
            var key = FormatCacheKey(urlPath);
            resource = CacheManager.Get(key) as IRedirect;
            return resource != null;
        }

        public void Add(UrlPath urlPath, IRedirect redirect)
            => CacheManager.Insert(FormatCacheKey(urlPath), redirect, new CacheEvictionPolicy(Enumerable.Empty<string>(), new[] {MasterPolicyKey}));

        private static string FormatCacheKey(UrlPath path) => $"{MasterPolicyKey}_{path}";
    }
}