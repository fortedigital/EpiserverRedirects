using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Repository.ControllerRepository
{
    public class TestRedirectRuleControllerRepository : RedirectRuleControllerRepository
    {
        private readonly Dictionary<Guid, RedirectRule> _redirectsDictionary;

        public TestRedirectRuleControllerRepository()
        {
            _redirectsDictionary = new Dictionary<Guid, RedirectRule>();
        }
        public TestRedirectRuleControllerRepository(Dictionary<Guid, RedirectRule> redirectsCollection)
        {
            _redirectsDictionary = redirectsCollection;
        }

        public override IQueryable<RedirectRule> Get()
        {
            return _redirectsDictionary.Select(r=>r.Value).AsQueryable();
        }

        public override RedirectRule GetById(Guid id)
        {
            _redirectsDictionary.TryGetValue(id, out var redirectRule);
            return redirectRule;
        }

        public override RedirectRule Add(RedirectRule redirectRule)
        {            
            redirectRule.Id = Identity.NewIdentity();
            _redirectsDictionary.Add(redirectRule.Id.ExternalId, redirectRule);

            return redirectRule;
        }

        public override RedirectRule Update(RedirectRule redirectRule)
        {
            _redirectsDictionary.TryGetValue(redirectRule.Id.ExternalId, out var redirectToUpdate);
            
            if(redirectToUpdate==null)
                throw new KeyNotFoundException("No existing redirect with this GUID");
            
            WriteToModel(redirectRule, redirectToUpdate);
            return redirectRule;
        }

        public override bool Delete(Guid id)
        {
            return _redirectsDictionary.Remove(id);
        }
    }
}