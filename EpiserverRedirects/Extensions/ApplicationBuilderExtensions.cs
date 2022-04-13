using Forte.EpiserverRedirects.Caching;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Events;
using Forte.EpiserverRedirects.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Forte.EpiserverRedirects.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEpiserverRedirects(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(RedirectMiddleware));

            var redirectsOptions = app.ApplicationServices.GetRequiredService<RedirectsOptions>();

            if (redirectsOptions.AddAutomaticRedirects)
            {
                var redirectsEventsRegistry = app.ApplicationServices.GetRequiredService<AutomaticRedirectsEventsRegistry>();
                redirectsEventsRegistry.RegisterEvents();
            }

            if (redirectsOptions.Caching.AllRedirectsCacheEnabled || redirectsOptions.Caching.UrlRedirectCacheEnabled)
            {
                var cachingEventsRegistry = app.ApplicationServices.GetRequiredService<CachingEventsRegistry>();
                cachingEventsRegistry.RegisterEvents();
            }

            return app;
        }
    }
}
