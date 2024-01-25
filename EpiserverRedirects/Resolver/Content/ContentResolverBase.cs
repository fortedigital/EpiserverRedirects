using EPiServer;
using EPiServer.Core;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Resolver.Content;

public abstract class ContentResolverBase
{
    private readonly IContentLoader _contentLoader;

    protected ContentResolverBase(IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    public bool TryGet(IRedirectRule rule, out IContent content)
    {
        var contentReference = GetReference(rule);
        return _contentLoader.TryGet(contentReference, out content);
    }
    
    protected abstract ContentReference GetReference(IRedirectRule rule);
}