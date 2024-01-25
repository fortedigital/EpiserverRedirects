using System;
using EPiServer;
using EPiServer.Core;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Resolver.Content;

public class CmsContentResolver : ContentResolverBase
{
    public CmsContentResolver(IContentLoader contentLoader) : base(contentLoader)
    {
    }

    protected override ContentReference GetReference(IRedirectRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule.ContentId);
        
        return new ContentReference(rule.ContentId.Value);
    }
}