using System;
using System.Linq;
using EPiServer.Data.Dynamic;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Repository
{
    public class DynamicDataStoreRepository : RedirectRuleRepository
    {
        private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;
        private DynamicDataStore DynamicDataStore => _dynamicDataStoreFactory.CreateStore(typeof(RedirectRule));

        private IQueryable<RedirectRule> GetAllByOldPattern(string oldPattern)
        {
            return DynamicDataStore.Items<RedirectRule>().Where(x => x.OldPattern == oldPattern);
        }
        
        private IQueryable<RedirectRule> RemoveDuplicates(RedirectRule redirectRule)
        {
            var oldPatternRedirectRules = 
                GetAllByOldPattern(redirectRule.OldPattern)
                    .Where(x => x.RedirectRuleType != RedirectRuleType.Regex)
                    .OrderByDescending(x => x.CreatedOn);
            
            IQueryable<RedirectRule> duplicates  = null;
            
            if (oldPatternRedirectRules.Count() > 1)
            {
                duplicates = oldPatternRedirectRules.Skip(1);

                foreach (var duplicate in duplicates)
                {
                    Delete(duplicate.Id.ExternalId);
                }
            }
            return duplicates;
        }
        public DynamicDataStoreRepository(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            _dynamicDataStoreFactory = dynamicDataStoreFactory;
        }

        public override IQueryable<RedirectRule> GetAll()
        {
            return DynamicDataStore.Items<RedirectRule>();
        }
        
        public override RedirectRule GetById(Guid id)
        {
            return DynamicDataStore.Items<RedirectRule>().FirstOrDefault(r => r.Id.ExternalId == id);
        }

        public override RedirectRule Add(RedirectRule redirectRule)
        {
            DynamicDataStore.Save(redirectRule);
            RemoveDuplicates(redirectRule);

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

        public override bool RemoveAllDuplicates()
        {
            var redirectRules = GetAll();
        
            try
            {
                foreach (var redirectRule in redirectRules)
                {
                    RemoveDuplicates(redirectRule);
                }
                return true;
            }
            catch
            {
                return false;
            }
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

        public override bool ClearAll()
        {
            try
            {
                DynamicDataStore.DeleteAll();
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
