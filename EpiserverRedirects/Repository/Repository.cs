using System;
using System.Linq;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Repository
{
    public interface IRedirectRuleRepository
    {
        RedirectRule GetById(Guid id);
        IQueryable<RedirectRule> GetAll();
        RedirectRule Add(RedirectRule redirectRule);
        RedirectRule Update(RedirectRule redirectRule);
        bool RemoveAllDuplicates();
        bool Delete(Guid id);
        bool ClearAll();
    }

    public abstract class RedirectRuleRepository : IRedirectRuleRepository
    {
        public abstract RedirectRule GetById(Guid id);
        public abstract IQueryable<RedirectRule> GetAll();
        public abstract RedirectRule Add(RedirectRule redirectRule);
        public abstract RedirectRule Update(RedirectRule redirectRule);
        public abstract bool RemoveAllDuplicates();
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
