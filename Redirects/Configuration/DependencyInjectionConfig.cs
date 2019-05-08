using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Repository;
using Forte.RedirectMiddleware.Request;
using Forte.RedirectMiddleware.Resolver;

namespace Forte.RedirectMiddleware.Configuration
{
    [InitializableModule]
    public class DependencyInjectionConfig : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddScoped<RequestHandler>();
            context.Services.AddTransient<IRedirectRuleRepository, DynamicDataStoreRepository>();
            context.Services.AddTransient<IResponseStatusCodeResolver, Http_1_1_ResponseStatusCodeResolver>();

            context.Services.AddTransient<IRedirectRuleResolver>(c => new CompositeResolver(
                new ExactMatchResolver(c.GetInstance<IQueryable<RedirectRule>>()),
                new RegexResolver(c.GetInstance<IQueryable<RedirectRule>>()),
                new WildcardResolver(c.GetInstance<IQueryable<RedirectRule>>())));
        }

        public void Initialize(InitializationEngine context) { }
        public void Uninitialize(InitializationEngine context) { }
    }
}