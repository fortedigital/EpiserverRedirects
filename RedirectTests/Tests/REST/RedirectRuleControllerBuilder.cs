using System;
using Forte.RedirectMiddleware.Controller;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.Mapper;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Repository;
using Forte.RedirectMiddleware.Repository.ControllerRepository;

namespace RedirectTests.Tests.REST
{
    public class RedirectRuleControllerBuilder : BaseBuilder<RedirectRuleController>
    {
        private IRedirectRuleMapper _redirectRuleMapper = new RedirectRuleMapper();
            
        public RedirectRuleControllerBuilder WithMapper(Func<RedirectRule, RedirectRuleDto> mapper)
        {
            _redirectRuleMapper = new RedirectRuleTestMapper(mapper);
            return this;
        }
        
        public override RedirectRuleController Create()
        {
            var existingRules = RedirectRuleTestDataBuilder.GetData();
            RedirectRuleControllerRepository = new TestRedirectRuleControllerRepository(existingRules);
            return new RedirectRuleController(RedirectRuleControllerRepository, _redirectRuleMapper);
        }
    }
}