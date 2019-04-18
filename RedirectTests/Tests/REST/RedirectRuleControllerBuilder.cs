using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.Mapper;
using Forte.RedirectMiddleware.Repository;
using Forte.RedirectMiddleware.REST;

namespace RedirectTests.Tests.REST
{
    public class RedirectRuleControllerBuilder : BaseBuilder<RedirectRuleController>
    {
        private IRedirectRuleMapper _redirectRuleMapper = new RedirectRuleMapper();
            
        public RedirectRuleControllerBuilder WithTestMapping()
        {
            _redirectRuleMapper = new RedirectRuleTestMapper();
            return this;
        }
        
        public RedirectRuleControllerBuilder WithOrdinaryMapping()
        {
            _redirectRuleMapper = new RedirectRuleMapper();
            return this;
        }
        
        public override RedirectRuleController Create()
        {
            var existingRules = RedirectRuleTestDataBuilder.GetData();
            RedirectRuleRepository = new TestRedirectRuleRepository(existingRules);
            return new RedirectRuleController(RedirectRuleRepository, _redirectRuleMapper);
        }
    }
}