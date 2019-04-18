using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data;
using Forte.RedirectMiddleware.Model;

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

        public override IQueryable<RedirectRule> GetAllRedirectRules()
        {
            return _redirectsDictionary.Select(r=>r.Value).AsQueryable();
        }

        public override RedirectRuleDto CreateRedirect(RedirectRuleDto redirectRuleDTO)
        {
            var redirect = RedirectRuleMapper.DtoToModel(redirectRuleDTO);
            
            redirect.Id = Identity.NewIdentity();
            _redirectsDictionary.Add(redirect.Id.ExternalId, redirect);

            redirectRuleDTO.Id = redirect.Id;
            return redirectRuleDTO;
        }

        public override RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectRuleDTO)
        {
            _redirectsDictionary.TryGetValue(redirectRuleDTO.Id.ExternalId, out var redirect);
            
            if(redirect==null)
                throw new KeyNotFoundException("No existing redirect with this GUID");
            
            RedirectRuleMapper.DtoToModel(redirectRuleDTO, redirect);
            return redirectRuleDTO;
        }

        public override bool DeleteRedirect(Guid id)
        {
            return _redirectsDictionary.Remove(id);
        }
    }
}