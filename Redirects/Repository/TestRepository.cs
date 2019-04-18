using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.Mapper;

namespace Forte.RedirectMiddleware.Repository
{
    public class TestRedirectRuleRepository : RedirectRuleRepository
    {
        private readonly Dictionary<Guid, RedirectRule> _redirectsDictionary;

        public TestRedirectRuleRepository()
        {
            _redirectsDictionary = new Dictionary<Guid, RedirectRule>();
        }
        public TestRedirectRuleRepository(Dictionary<Guid, RedirectRule> redirectsCollection)
        {
            _redirectsDictionary = redirectsCollection;
        }

        public override RedirectRule GetRedirectRule(UrlPath oldPath)
        {
            var redirect = _redirectsDictionary.FirstOrDefault(r => r.Value.OldPath == oldPath).Value;

            return redirect;
        }

        public override IEnumerable<RedirectRule> GetAllRedirectRules()
        {
            return _redirectsDictionary.Select(r=>r.Value);
        }

        public override RedirectRule GetRedirectRule(Guid id)
        {
            _redirectsDictionary.TryGetValue(id, out var redirectRule);
            return redirectRule;
        }

        public override RedirectRule CreateRedirect(RedirectRule redirectRule)
        {            
            redirectRule.Id = Identity.NewIdentity();
            _redirectsDictionary.Add(redirectRule.Id.ExternalId, redirectRule);

            return redirectRule;
        }

        public override RedirectRule UpdateRedirect(RedirectRule redirectRule)
        {
            _redirectsDictionary.TryGetValue(redirectRule.Id.ExternalId, out var redirectToUpdate);
            
            if(redirectToUpdate==null)
                throw new KeyNotFoundException("No existing redirect with this GUID");
            
            WriteToModel(redirectRule, redirectToUpdate);
            return redirectRule;
        }

        public override bool DeleteRedirect(Guid id)
        {
            return _redirectsDictionary.Remove(id);
        }
    }
}