using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.Mapper;

namespace Forte.RedirectMiddleware.Repository
{
    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleRepository))]
    public class DynamicDataStoreRedirectRuleRepository : RedirectRuleRepository
    {
        private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;
        private readonly DynamicDataStore _dynamicDataStore;

        public DynamicDataStoreRedirectRuleRepository(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            _dynamicDataStoreFactory = dynamicDataStoreFactory;
            _dynamicDataStore = CreateStore();
        }
        
        private DynamicDataStore CreateStore()
        {
            return _dynamicDataStoreFactory.CreateStore(typeof(RedirectRule));
        }
        
        public override RedirectRule GetRedirectRule(UrlPath oldPath)
        {
            var redirect = _dynamicDataStore.Items<RedirectRule>().FirstOrDefault(r => r.OldPath == oldPath);

            return redirect;
        }

        public override IEnumerable<RedirectRule> GetAllRedirectRules()
        {
            return _dynamicDataStore.Items<RedirectRule>().AsEnumerable();
        }

        public override RedirectRule GetRedirectRule(Guid id)
        {
            return _dynamicDataStore.Items<RedirectRule>().FirstOrDefault(r => r.Id.ExternalId == id);
        }

        public override RedirectRule CreateRedirect(RedirectRule redirectRule)
        {     
            _dynamicDataStore.Save(redirectRule);
            return redirectRule;
        }

        public override RedirectRule UpdateRedirect(RedirectRule redirectRule)
        {
            var redirectToUpdate = GetRedirectRule(redirectRule.Id.ExternalId);
            
            if(redirectToUpdate==null)
                throw new Exception("No existing redirect with this GUID");
            
            WriteToModel(redirectRule, redirectToUpdate);
            
            _dynamicDataStore.Save(redirectToUpdate);

            return redirectToUpdate;
        }

        public override bool DeleteRedirect(Guid id)
        {
            try
            {
                _dynamicDataStore.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}