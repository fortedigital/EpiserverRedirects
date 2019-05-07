using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
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
            context.Services.AddTransient<RequestHandler>();
            context.Services.AddTransient<IRedirectRuleRepository, DynamicDataStoreRepository>();
            context.Services.AddTransient<IResponseStatusCodeResolver, Http_1_1_ResponseStatusCodeResolver>();

            context.Services.AddTransient<IRedirectRuleResolver, CompositeResolver>();

            context.Services.AddTransient<IRedirectRuleResolver, ExactMatchResolver>();
            context.Services.AddTransient<IRedirectRuleResolver, RegexResolver>();
            context.Services.AddTransient<IRedirectRuleResolver, WildcardResolver>();
        }

        public void Initialize(InitializationEngine context) { }
        public void Uninitialize(InitializationEngine context) { }
    }
}