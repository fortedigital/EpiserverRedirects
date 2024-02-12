using System;
using EPiServer;
using EPiServer.Core;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Resolver.Content;

public abstract class RedirectContentResolverBase
{
    private readonly IContentLoader _contentLoader;

    public abstract Guid ProviderId { get; }
    public abstract string ProviderKey { get; }
    public abstract string ProviderName { get; }

    protected RedirectContentResolverBase(IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    public bool TryGet(IRedirectRule rule, out IContent content)
    {
        if (rule.ContentId.HasValue)
        {
            var contentReference = new ContentReference(rule.ContentId.Value, ProviderKey);
            return _contentLoader.TryGet(contentReference, out content);
        }

        content = null;
        return false;
    }
}