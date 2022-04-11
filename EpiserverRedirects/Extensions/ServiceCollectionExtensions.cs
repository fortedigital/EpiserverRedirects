﻿using System;
using System.Linq;
using EPiServer;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Modules;
using Forte.EpiserverRedirects.Caching;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Events;
using Forte.EpiserverRedirects.Extensions;
using Forte.EpiserverRedirects.Import;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Repository;
using Forte.EpiserverRedirects.Request;
using Forte.EpiserverRedirects.Resolver;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEpiserverRedirects(this IServiceCollection services, IConfiguration configuration)
        {
            var redirectsOptions = configuration.GetSection<RedirectsOptions>(Constants.AppSettingsRootSectionName);

            return AddEpiserverRedirectsInternal(services, redirectsOptions);
        }

        public static IServiceCollection AddEpiserverRedirects(this IServiceCollection services, Action<RedirectsOptions> configureAction)
        {
            var redirectsOptions = new RedirectsOptions();

            configureAction?.Invoke(redirectsOptions);

            return AddEpiserverRedirectsInternal(services, redirectsOptions);
        }

        private static IServiceCollection AddEpiserverRedirectsInternal(IServiceCollection services, RedirectsOptions redirectsOptions)
        {
            services.AddSingleton(redirectsOptions);
            services.AddSingleton(redirectsOptions.Caching);
            services.AddScoped<RequestHandler>();
            services.AddTransient<IRedirectRuleMapper, RedirectRuleMapper>();
            services.AddTransient<RedirectsLoader>();
            services.AddTransient<RedirectsImporter>();

            if (redirectsOptions.AddAutomaticRedirects)
            {
                services.AddSingleton<AutomaticRedirectsEventsRegistry>();
            }

            if (redirectsOptions.Caching.AllRedirectsCacheEnabled || redirectsOptions.Caching.UrlRedirectCacheEnabled)
            {
                services.AddSingleton<CachingEventsRegistry>();
                services.AddTransient<ICacheRemover, CacheRemover>();
                services.AddTransient<ICache, Cache>();
            }

            if (redirectsOptions.Caching.AllRedirectsCacheEnabled)
            {
                services.AddTransient<IRedirectRuleRepository>(
                    provider => new RedirectRuleCachedRepositoryDecorator(
                        new DynamicDataStoreRepository(provider.GetService<DynamicDataStoreFactory>()),
                        provider.GetService<ICache>()));
            }
            else
            {
                services.AddTransient<IRedirectRuleRepository, DynamicDataStoreRepository>();
            }

            if (redirectsOptions.Caching.UrlRedirectCacheEnabled)
            {
                services.AddTransient<IRedirectRuleResolver>(provider => new CacheRedirectResolverDecorator(GetCompositeRuleResolver(provider), provider.GetService<ICache>()));
            }
            else
            {
                services.AddTransient(GetCompositeRuleResolver);
            }

            services.Configure<ProtectedModuleOptions>(
                options =>
                {
                    if (!options.Items.Any(x => x.Name.Equals(Constants.ModuleName)))
                    {
                        options.Items.Add(
                            new ModuleDetails
                            {
                                Name = Constants.ModuleName
                            });
                    }
                });

            return services;
        }

        private static IRedirectRuleResolver GetCompositeRuleResolver(IServiceProvider provider)
        {
            var contentLoader = provider.GetService<IContentLoader>();

            return new CompositeResolver(
                new ExactMatchResolver(provider.GetInstance<IRedirectRuleRepository>(), contentLoader),
                new RegexResolver(provider.GetInstance<IRedirectRuleRepository>(), contentLoader));
        }
    }
}