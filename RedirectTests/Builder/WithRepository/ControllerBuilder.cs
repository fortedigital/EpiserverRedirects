using System;
using Forte.Redirects.Mapper;
using Forte.Redirects.Menu;
using Forte.Redirects.Model.RedirectRule;
using Forte.RedirectTests.Mapper;

namespace Forte.RedirectTests.Builder.WithRepository
{
    public class ControllerBuilder : BaseWithRepositoryBuilder<RedirectRuleStore, ControllerBuilder>
    {
        protected override ControllerBuilder ThisBuilder => this;
        
        private IRedirectRuleMapper _redirectRuleMapper = new RedirectRuleMapper();
            
        public ControllerBuilder WithMapper(Func<RedirectRule, RedirectRuleDto> mapper)
        {
            _redirectRuleMapper = new RedirectRuleTestMapper(mapper);
            return this;
        }
        
        public override RedirectRuleStore Create()
        {
            CreateRepository();
            return new RedirectRuleStore(RedirectRuleRepository, _redirectRuleMapper);
        }
    }
}