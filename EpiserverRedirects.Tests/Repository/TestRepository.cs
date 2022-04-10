using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Tests.Repository
{
    public class TestRepository : RedirectRuleRepository
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


        public override RedirectRule GetById(Guid id)
        {
            var redirectRule =
                _redirectsHashSet.FirstOrDefault(r => r.Id == id);
            return redirectRule;
        }

        public override IQueryable<RedirectRule> GetAll() => _redirectsHashSet.AsQueryable();
        
        public override RedirectRule Add(RedirectRule redirectRule)
        {            
            redirectRule.Id = Identity.NewIdentity();
            _redirectsHashSet.Add(redirectRule);

            return redirectRule;
        }

        public override RedirectRule Update(RedirectRule redirectRule)
        {
            var redirectRuleToUpdate =
                _redirectsHashSet.FirstOrDefault(r => r.Id == redirectRule.Id);
            
            if(redirectRuleToUpdate==null)
                throw new KeyNotFoundException("No existing redirect with this GUID");
            
            WriteToModel(redirectRule, redirectRuleToUpdate);
            return redirectRule;
        }

        public override bool Delete(Guid id)
        {
            var redirectRule = _redirectsHashSet.FirstOrDefault(r => r.Id.ExternalId == id);
            
            return redirectRule != null && _redirectsHashSet.Remove(redirectRule);
        }

        public override bool ClearAll()
        {
            _redirectsHashSet.Clear();

            return true;
        }
    }
}
