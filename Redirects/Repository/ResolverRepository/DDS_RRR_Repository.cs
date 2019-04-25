using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;

namespace Forte.RedirectMiddleware.Repository.ResolverRepository
{
    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleResolverRepository))]
    public class DynamicDataStoreRedirectRuleResolverRepository : RedirectRuleResolverRepository
    {
        private readonly DynamicDataStore _dynamicDataStore;

        public DynamicDataStoreRedirectRuleResolverRepository(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            _dynamicDataStore = dynamicDataStoreFactory.CreateStore(typeof(RedirectRule));
        }

        public override RedirectRule FindByOldPath(UrlPath oldPath)
        {
            var redirect = _dynamicDataStore.Items<RedirectRule>().FirstOrDefault(r => r.OldPath == oldPath);
            return redirect;
        }
    }
}