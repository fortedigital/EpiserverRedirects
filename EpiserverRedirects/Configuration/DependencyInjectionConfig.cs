using System.Linq;
using EPiServer;
using EPiServer.Data.Dynamic;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Forte.EpiserverRedirects.Request;
using Forte.EpiserverRedirects.Resolver;
using Forte.EpiserverRedirects.System;

namespace Forte.EpiserverRedirects.Configuration
{
    [InitializableModule]
    public class DependencyInjectionConfig : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddScoped<RequestHandler>();

            context.Services.AddTransient<IRedirectRuleRepository>(c =>
                new RedirectRuleCachedRepositoryDecorator(new DynamicDataStoreRepository(c.GetInstance<DynamicDataStoreFactory>()),
                    c.GetInstance<ICache<RedirectRule[]>>()));
            context.Services.AddTransient<IResponseStatusCodeResolver, Http_1_1_ResponseStatusCodeResolver>();

            context.Services.AddTransient<IRedirectRuleMapper, RedirectRuleMapper>();

            context.Services.AddTransient<ICacheRemover, CacheRemover>();
            context.Services.AddTransient<ICache<RedirectRule[]>, Cache<RedirectRule[]>>();
            context.Services.AddTransient(CreateCompositeRuleResolver);
        }

        private IRedirectRuleResolver CreateCompositeRuleResolver(IServiceLocator c)
        {
            return new CompositeResolver(
                new ExactMatchResolver(c.GetInstance<IRedirectRuleRepository>(), c.GetInstance<IContentLoader>()),
                new RegexResolver(c.GetInstance<IRedirectRuleRepository>(), c.GetInstance<IContentLoader>()
                    /*,new WildcardResolver(c.GetInstance<IQueryable<RedirectRule>>())*/));
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}