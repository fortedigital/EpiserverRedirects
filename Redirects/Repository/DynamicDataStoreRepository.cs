using System;
using System.Linq;
using EPiServer.Data.Dynamic;
using Forte.Redirects.Model.RedirectRule;

namespace Forte.Redirects.Repository
{
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
            var redirectRuleToUpdate = GetById(redirectRule.Id.ExternalId);
            
            if(redirectRuleToUpdate==null)
                throw new Exception("No existing redirect with this GUID");
            
            WriteToModel(redirectRule, redirectRuleToUpdate);
            
            DynamicDataStore.Save(redirectRuleToUpdate);

            return redirectRuleToUpdate;
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