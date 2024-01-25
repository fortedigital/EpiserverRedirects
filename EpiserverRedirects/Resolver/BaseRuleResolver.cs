using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.Filters;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;
using Forte.EpiserverRedirects.Resolver.Content;

namespace Forte.EpiserverRedirects.Resolver
{
    public abstract class BaseRuleResolver
    {
        private readonly IEnumerable<ContentResolverBase> _contentResolvers;

        protected BaseRuleResolver(IEnumerable<ContentResolverBase> contentResolvers)
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

            IContent content = null;
            if (_contentResolvers.All(contentResolver => !contentResolver.TryGet(rule, out content)))
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
