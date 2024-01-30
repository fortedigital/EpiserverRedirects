using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Filters;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;
using Forte.EpiserverRedirects.Resolver.Content;

namespace Forte.EpiserverRedirects.Resolver
{
    public abstract class BaseRuleResolver
    {
        private readonly IEnumerable<RedirectContentResolverBase> _contentResolvers;

        protected BaseRuleResolver(IEnumerable<RedirectContentResolverBase> contentResolvers)
        {
            _contentResolvers = contentResolvers;
        }

        protected IRedirect ResolveRule<T>(IRedirectRule rule, Func<IRedirectRule, T> constructRedirect) where T : IRedirect
        {
            if (rule == null)
            {
                return new NullRedirectRule();
            }

            if (!rule.ContentId.HasValue)
            {
                return constructRedirect(rule);
            }

            var contentResolver = _contentResolvers
                .FirstOrDefault(contentResolver => contentResolver.ProviderKey == rule.ContentProviderKey);

            if (contentResolver is null || !contentResolver.TryGet(rule, out var content))
            {
                return new NullRedirectRule();
            }
            
            var filter = new FilterPublished();
            if (filter.ShouldFilter(content))
            {
                return new NullRedirectRule();
            }

            return constructRedirect(rule);
        }
    }
}
