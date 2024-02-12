using System;
using EPiServer;

namespace Forte.EpiserverRedirects.Resolver.Content;

public class DefaultRedirectContentResolver : RedirectContentResolverBase
{
    public override Guid ProviderId { get; } = new("D6FCD62B-E76D-4178-BDAE-7D9A6F3708AB");
    public override string ProviderKey => null;
    public override string ProviderName => "Default";

    public DefaultRedirectContentResolver(IContentLoader contentLoader) : base(contentLoader)
    {
    }
}