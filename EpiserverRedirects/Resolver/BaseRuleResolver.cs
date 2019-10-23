using System;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.Resolver
{
    public abstract class BaseRuleResolver
    {
        private readonly IContentLoader _contentLoader;

        protected BaseRuleResolver(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        protected IRedirect ResolveRule<T>(RedirectRule rule, Func<RedirectRule, T> constructRedirect) where T : IRedirect
        {
            if (rule == null)
            {
                return new NullRedirectRule();
            }

            if (!rule.ContentId.HasValue) return constructRedirect(rule);

            if (!_contentLoader.TryGet<PageData>(new ContentReference(rule.ContentId.Value), out var content))
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
