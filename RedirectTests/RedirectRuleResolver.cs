using System;
using System.Collections.Generic;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Repository;
using Forte.RedirectMiddleware.Service;

namespace RedirectTests
{
    public class RedirectRuleResolver
    {
        private IRedirectRuleRepository _redirectRuleRepository;
        private IRedirectService _redirectService;
        
        private RedirectRuleResolver() { }
        
        public static RedirectRuleResolver Register()
        {
            return new RedirectRuleResolver();
        }

        public RedirectRuleResolver WithExistingRules(Dictionary<Guid, RedirectRule> existingRedirects)
        {
            _redirectRuleRepository = new TestRedirectRuleRepository(existingRedirects);
            return this;
        }
        
        public RedirectRuleResolver WithNoRules()
        {
            _redirectRuleRepository = new TestRedirectRuleRepository();
            return this;
        }

        public RedirectRuleResolver Create()
        {
            _redirectService= new RedirectService(_redirectRuleRepository);
            return this;
        }

        public IRedirectService Resolve()
        {
            return _redirectService;
        }
    }
}