using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forte.Redirects.Model.UrlPath;
using Forte.Redirects.Redirect;

namespace Forte.Redirects.Resolver
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
            
            if (redirectRule == null)
                return null;

            return await redirectRule;
        }
        
    }
}