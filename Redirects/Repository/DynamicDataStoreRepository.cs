using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using EPiServer.Web.Internal;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;

namespace Forte.RedirectMiddleware.Repository
{
//    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleRepository))]
    public class DynamicDataStoreRepository : RedirectRuleRepository
    {
        private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;
        private DynamicDataStore DynamicDataStore => _dynamicDataStoreFactory.CreateStore(typeof(RedirectRule));

        private void InitItems()
        {
            Items = DynamicDataStore.Items<RedirectRule>();
        }
        
        public DynamicDataStoreRepository(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            _dynamicDataStoreFactory = dynamicDataStoreFactory;
            InitItems();
        }

        public override RedirectRule GetById(Guid id)
        {
            return DynamicDataStore.Items<RedirectRule>().FirstOrDefault(r => r.Id.ExternalId == id);
        }

        public override RedirectRule Add(RedirectRule redirectRule)
        {     
            DynamicDataStore.Save(redirectRule);
            return redirectRule;
        }

        public override RedirectRule Update(RedirectRule redirectRule)
        {
            var redirectToUpdate = GetById(redirectRule.Id.ExternalId);
            
            if(redirectToUpdate==null)
                throw new Exception("No existing redirect with this GUID");
            
            WriteToModel(redirectRule, redirectToUpdate);
            
            DynamicDataStore.Save(redirectToUpdate);

            return redirectToUpdate;
        }

        public override bool Delete(Guid id)
        {
            try
            {
                DynamicDataStore.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}