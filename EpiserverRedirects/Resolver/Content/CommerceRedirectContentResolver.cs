using System;
using EPiServer;

namespace Forte.EpiserverRedirects.Resolver.Content;

public class CommerceRedirectContentResolver : RedirectContentResolverBase
{
    public override Guid ProviderId => new("18552A8A-90B8-4F8D-886A-C2B673B11294");
    public override string ProviderKey => "CatalogContent";
    public override string ProviderName => "Commerce";

    public CommerceRedirectContentResolver(IContentLoader contentLoader) : base(contentLoader)
    {
    }
}