using EPiServer;
using EPiServer.Data.Dynamic;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Forte.EpiserverRedirects.Encoder;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;
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

            context.Services.AddTransient<IResponseStatusCodeResolver, Http_1_1_ResponseStatusCodeResolver>();
            context.Services.AddTransient<IRedirectRuleMapper, RedirectRuleMapper>();
            context.Services.AddTransient<IUrlPathEncoder, UrlPathSpaceEncoder>();

            context.Services.AddTransient<ICacheRemover, CacheRemover>();
            context.Services.AddTransient<ICacheConfiguration, CacheConfiguration>();
            RegisterRepository(context);
            RegisterRedirectResolver(context);
        }

        private void RegisterRedirectResolver(ServiceConfigurationContext context)
        {
            if (Configuration.IsUrlRedirectCacheEnabled)
            {
                context.Services.AddTransient<ICache<IRedirect>, Cache<IRedirect>>();
                context.Services.AddTransient<IRedirectRuleResolver>(
                    c => new CacheRedirectResolverDecorator(CreateCompositeRuleResolver(c), c.GetInstance<ICache<IRedirect>>()));
            }
            else
            {
                context.Services.AddTransient(CreateCompositeRuleResolver);
            }
        }

        private static void RegisterRepository(ServiceConfigurationContext context)
        {
            if (Configuration.IsAllRedirectsCacheEnabled)
            {
                context.Services.AddTransient<ICache<RedirectRule[]>, Cache<RedirectRule[]>>();
                context.Services.AddTransient<IRedirectRuleRepository>(c =>
                    new RedirectRuleCachedRepositoryDecorator(new DynamicDataStoreRepository(c.GetInstance<DynamicDataStoreFactory>()),
                        c.GetInstance<ICache<RedirectRule[]>>()));
            }
            else
            {
                context.Services.AddTransient<IRedirectRuleRepository, DynamicDataStoreRepository>();
            }
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
