using System;
using Forte.RedirectMiddleware.Controller;
using Forte.RedirectMiddleware.Model.Mapper;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace RedirectTests.Tests.Builder
{
    public class ControllerBuilder : BaseBuilder<RedirectRuleController, ControllerBuilder>
    {
        protected override ControllerBuilder ThisBuilder => this;
        
        private IRedirectRuleMapper _redirectRuleMapper = new RedirectRuleMapper();
            
        public ControllerBuilder WithMapper(Func<RedirectRule, RedirectRuleDto> mapper)
        {
            _redirectRuleMapper = new RedirectRuleTestMapper(mapper);
            return this;
        }
        
        public override RedirectRuleController Create()
        {
            CreateRepository();
            return new RedirectRuleController(RedirectRuleRepository, _redirectRuleMapper);
        }
    }
}