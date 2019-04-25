using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Repository
{
    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleControllerRepository))]
    public class DynamicDataStoreRepository : RedirectRuleRepository
    {
        private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;
        private readonly DynamicDataStore _dynamicDataStore;

        private void InitQueryable()
        {
            var queryable = _dynamicDataStore.Items<RedirectRule>();
            base.InitQueryable(queryable);
        }
        
        public DynamicDataStoreRepository(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            _dynamicDataStoreFactory = dynamicDataStoreFactory;
            _dynamicDataStore = CreateStore();
            InitQueryable();
        }
        
        private DynamicDataStore CreateStore()
        {
            return _dynamicDataStoreFactory.CreateStore(typeof(RedirectRule));
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

        public override IEnumerator<RedirectRule> GetEnumerator()
        {
            return _dynamicDataStore.Items<RedirectRule>().GetEnumerator();
        }


    }
}