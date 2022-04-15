using System;
using System.Linq;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Repository
{
    public abstract class RedirectRuleRepository : IRedirectRuleRepository
    {
        public abstract RedirectRule GetById(Guid id);
        public abstract IQueryable<RedirectRule> GetAll();
        public abstract RedirectRule Add(RedirectRule redirectRule);
        public abstract RedirectRule Update(RedirectRule redirectRule);
        public abstract bool Delete(Guid id);
        public abstract bool ClearAll();

        protected static void WriteToModel(RedirectRule redirectRule, RedirectRule redirectRuleToUpdate)
        {
            redirectRuleToUpdate.Id = redirectRule.Id;
            redirectRuleToUpdate.OldPattern = redirectRule.OldPattern;
            redirectRuleToUpdate.NewPattern = redirectRule.NewPattern;
            redirectRuleToUpdate.RedirectType = redirectRule.RedirectType;
            redirectRuleToUpdate.RedirectRuleType = redirectRule.RedirectRuleType;
            redirectRuleToUpdate.IsActive = redirectRule.IsActive;
            redirectRuleToUpdate.Notes = redirectRule.Notes;
            redirectRuleToUpdate.Priority = redirectRule.Priority;
            redirectRuleToUpdate.ContentId = redirectRule.ContentId;
        }
    }
}
