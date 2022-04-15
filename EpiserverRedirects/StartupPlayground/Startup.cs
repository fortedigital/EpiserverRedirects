using Forte.EpiserverRedirects.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forte.EpiserverRedirects.StartupPlayground
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureEpiserverRedirects(
                    options =>
                    {
                        options.Caching.AllRedirectsCacheEnabled = true;
                        options.Caching.UrlRedirectCacheEnabled = true;

                        options.PreserveQueryString = false;
                        options.AddAutomaticRedirects = false;
                        options.SystemRedirectRulePriority = 80;
                        options.DefaultRedirectRulePriority = 100;
                    })
                .AddDynamicDataStoreRepository();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEpiserverRedirects();
        }
    }
}