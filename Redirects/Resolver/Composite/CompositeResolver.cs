using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Redirect.Base;
using Forte.RedirectMiddleware.Resolver.Base;

namespace Forte.RedirectMiddleware.Resolver.Composite
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CompositeResolver : IRedirectRuleResolver
    {
        private readonly List<IRedirectRuleResolver> _resolvers = new List<IRedirectRuleResolver>();
        public CompositeResolver(params IRedirectRuleResolver[] resolvers)
        {
            _resolvers.AddRange(resolvers);
        }

        public async Task<IRedirect> ResolveRedirectRule(UrlPath oldPath)
        {
            var redirectRule =  _resolvers
                .Select(resolver => resolver.ResolveRedirectRule(oldPath))
                .FirstOrDefault(redirectResult => redirectResult != null);

            return await redirectRule;
        }
        
    }
}