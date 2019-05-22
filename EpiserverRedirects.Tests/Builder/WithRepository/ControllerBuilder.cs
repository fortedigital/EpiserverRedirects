using System;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Menu;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Tests.Mapper;

namespace Forte.EpiserverRedirects.Tests.Builder.WithRepository
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