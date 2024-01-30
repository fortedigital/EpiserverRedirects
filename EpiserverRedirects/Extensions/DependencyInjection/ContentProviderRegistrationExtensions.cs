using System;
using System.Collections.Generic;
using System.Linq;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Resolver.Content;
using Microsoft.Extensions.DependencyInjection;

namespace Forte.EpiserverRedirects.Extensions.DependencyInjection;

public static class ContentProviderRegistrationExtensions
{
    public static IServiceCollection AddContentProviderResolvers(this IServiceCollection services)
    {
        services.AddTransient(typeof(RedirectContentResolverBase), typeof(DefaultRedirectContentResolver));
        services.AddTransient(typeof(RedirectContentResolverBase), typeof(CommerceRedirectContentResolver));

        services
            .AddOptions<ContentProvidersOptions>()
            .Configure<IEnumerable<RedirectContentResolverBase>>((o, redirectContentResolverBases) =>
            {
                var contentResolverBases = redirectContentResolverBases as RedirectContentResolverBase[] ?? redirectContentResolverBases.ToArray();
                if (contentResolverBases.Select(s => s.ProviderId).Distinct().Count() != contentResolverBases.Length)
                {
                    throw new Exception(
                        "Content resolvers provider ID is not unique. Please check all RedirectContentResolverBase descendants.");
                }
                    
                o.ContentProviders = contentResolverBases
                    .Select(r => new ContentProviderOption(r.ProviderId, r.ProviderKey, r.ProviderName))
                    .ToArray();
            });

        return services;
    }
}
