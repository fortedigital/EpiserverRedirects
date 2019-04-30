using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Data;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Repository
{
    public class TestRepository : RedirectRuleRepository
    {
        private readonly HashSet<RedirectRule> _redirectsHashSet;

        protected sealed override void InitQueryable()
        {
            var queryable = _redirectsHashSet.AsQueryable();
            base.InitQueryable(queryable);
        }
        public TestRepository()
        {
            _redirectsHashSet = new HashSet<RedirectRule>();
            InitQueryable();
        }
        public TestRepository(HashSet<RedirectRule> redirectsCollection)
        {
            _redirectsHashSet = redirectsCollection;
            InitQueryable();
        }

        public override RedirectRule GetById(Guid id)
        {
            var redirectRule =
                _redirectsHashSet.FirstOrDefault(r => r.Id == id);
            return redirectRule;
        }

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

        public override IEnumerator<RedirectRule> GetEnumerator()
        {
            return _redirectsHashSet.GetEnumerator();
        }
    }
}