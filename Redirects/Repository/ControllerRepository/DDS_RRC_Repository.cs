using System;
using System.Linq;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Repository.ControllerRepository
{
    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleControllerRepository))]
    public class DynamicDataStoreRedirectRuleControllerRepository : RedirectRuleControllerRepository
    {
        private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;
        private readonly DynamicDataStore _dynamicDataStore;

        public DynamicDataStoreRedirectRuleControllerRepository(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            _dynamicDataStoreFactory = dynamicDataStoreFactory;
            _dynamicDataStore = CreateStore();
        }
        
        private DynamicDataStore CreateStore()
        {
            return _dynamicDataStoreFactory.CreateStore(typeof(RedirectRule));
        }

        public override IQueryable<RedirectRule> Get()
        {
            return _dynamicDataStore.Items<RedirectRule>();
        }

        public override RedirectRule GetById(Guid id)
        {
            return _dynamicDataStore.Items<RedirectRule>().FirstOrDefault(r => r.Id.ExternalId == id);
        }

        public override RedirectRule Add(RedirectRule redirectRule)
        {     
            _dynamicDataStore.Save(redirectRule);
            return redirectRule;
        }

        public override RedirectRule Update(RedirectRule redirectRule)
        {
            var redirectToUpdate = GetById(redirectRule.Id.ExternalId);
            
            if(redirectToUpdate==null)
                throw new Exception("No existing redirect with this GUID");
            
            WriteToModel(redirectRule, redirectToUpdate);
            
            _dynamicDataStore.Save(redirectToUpdate);

            return redirectToUpdate;
        }

        public override bool Delete(Guid id)
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