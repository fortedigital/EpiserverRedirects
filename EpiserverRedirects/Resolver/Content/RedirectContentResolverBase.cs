using System;
using EPiServer;
using EPiServer.Core;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Resolver.Content;

public abstract class RedirectContentResolverBase
{
    private readonly IContentLoader contentLoader;

    public abstract Guid ProviderId { get; }
    public abstract string ProviderKey { get; }
    public abstract string ProviderName { get; }

    protected RedirectContentResolverBase(IContentLoader contentLoader)
    {
        this.contentLoader = contentLoader;
    }

    public bool TryGet(IRedirectRule rule, out IContent content)
    {
        var contentReference = new ContentReference(rule.ContentId.Value, ProviderKey);
        return contentLoader.TryGet(contentReference, out content);
    }
}