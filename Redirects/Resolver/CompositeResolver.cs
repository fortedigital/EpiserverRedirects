using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;

namespace Forte.RedirectMiddleware.Resolver
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CompositeResolver : IRedirectRuleResolver
    {
        private readonly ICollection<IRedirectRuleResolver> _resolvers = new List<IRedirectRuleResolver>();
        public CompositeResolver(IRedirectRuleResolver exactMatchResolver, IRedirectRuleResolver regexResolver, IRedirectRuleResolver wildcardResolver)
        {
            _resolvers.Add(exactMatchResolver);
            _resolvers.Add(regexResolver);
            _resolvers.Add(wildcardResolver);
        }

        public async Task<RedirectRule> ResolveRedirectRule(UrlPath oldPath)
        {
            var redirectRule =  _resolvers
                .Select(resolver => resolver.ResolveRedirectRule(oldPath))
                .FirstOrDefault(redirectResult => redirectResult != null);

            return await redirectRule;
        }
        
    }
}