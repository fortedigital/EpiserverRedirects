using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Forte.EpiserverRedirects.Tests.Repository
{
    public class TestRepository : IRedirectRuleRepository
    {
        private readonly HashSet<RedirectRule> _redirectsHashSet;
        
        public TestRepository()
        {
            _redirectsHashSet = new HashSet<RedirectRule>();
        }

        public TestRepository(HashSet<RedirectRule> redirectsCollection)
        {
            _redirectsHashSet = redirectsCollection;
        }

        public IRedirectRule GetById(Guid id)
        {
            var redirectRule =
                _redirectsHashSet.FirstOrDefault(r => r.RuleId == id);
            return redirectRule;
        }

        public IQueryable<IRedirectRule> GetAll() => _redirectsHashSet.AsQueryable();
        
        public IRedirectRule Add(IRedirectRule redirectRule)
        {
            (redirectRule as RedirectRule).RuleId = Guid.NewGuid();
            _redirectsHashSet.Add(redirectRule as RedirectRule);

            return redirectRule;
        }

        public IRedirectRule Update(IRedirectRule redirectRule)
        {
            var redirectRuleToUpdate =
                _redirectsHashSet.FirstOrDefault(r => r.RuleId == redirectRule.RuleId);
            
            if(redirectRuleToUpdate==null)
            {
                throw new KeyNotFoundException("No existing redirect with this GUID");
            }

            WriteToModel(redirectRule, redirectRuleToUpdate);
            return redirectRule;
        }

        public bool Delete(Guid id)
        {
            var redirectRule = _redirectsHashSet.FirstOrDefault(r => r.RuleId == id);
            
            return redirectRule != null && _redirectsHashSet.Remove(redirectRule);
        }

        public bool ClearAll()
        {
            _redirectsHashSet.Clear();

            return true;
        }

        protected static void WriteToModel(IRedirectRule redirectRule, IRedirectRule redirectRuleToUpdate)
        {
            (redirectRuleToUpdate as RedirectRule).RuleId = (redirectRule as RedirectRule).RuleId;
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
