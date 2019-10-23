using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPiServer.Cms.Shell.UI.Components;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.Resolver
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CompositeResolver : IRedirectRuleResolver
    {
        private readonly List<IRedirectRuleResolver> _resolvers = new List<IRedirectRuleResolver>();
        public CompositeResolver(params IRedirectRuleResolver[] resolvers)
        {
            _resolvers.AddRange(resolvers);
        }

        public Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            var resolverTasks = _resolvers.Select(x => x.ResolveRedirectRuleAsync(oldPath));

            return Task.WhenAll(resolverTasks.ToArray())
                .ContinueWith(continuationTask =>
                {
                    return continuationTask
                        .Result
                        .OrderBy(x => x.Priority)
                        .FirstOrDefault() ?? new NullRedirectRule();
                });
        }
    }
}
