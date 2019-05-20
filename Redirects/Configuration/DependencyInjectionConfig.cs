using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Forte.Redirects.Mapper;
using Forte.Redirects.Model;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Repository;
using Forte.Redirects.Request;
using Forte.Redirects.Resolver;

namespace Forte.Redirects.Configuration
{
    [InitializableModule]
    public class DependencyInjectionConfig : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddScoped<RequestHandler>();
            context.Services.AddTransient<IRedirectRuleRepository, DynamicDataStoreRepository>();
            context.Services.AddTransient<IResponseStatusCodeResolver, Http_1_1_ResponseStatusCodeResolver>();

            context.Services.AddTransient<IRedirectRuleMapper, RedirectRuleMapper>();

            context.Services.AddTransient<IRedirectRuleResolver>(c => new CompositeResolver(
                new ExactMatchResolver(c.GetInstance<IQueryable<RedirectRule>>()),
                new RegexResolver(c.GetInstance<IQueryable<RedirectRule>>()),
                new WildcardResolver(c.GetInstance<IQueryable<RedirectRule>>())));
        }

        public void Initialize(InitializationEngine context) { }
        public void Uninitialize(InitializationEngine context) { }
    }
}