using System;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Menu;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Forte.EpiserverRedirects.Tests.Builder.WithRepository
{
    public class ControllerBuilder : BaseWithRepositoryBuilder<RedirectRuleStore, ControllerBuilder>
    {
        protected override ControllerBuilder ThisBuilder => this;

        private IRedirectRuleMapper _redirectRuleMapper = new RedirectRuleMapper(new RedirectsOptions { DefaultRedirectRulePriority = 100 });
        private readonly Mock<ControllerContext> _controllerContext = new Mock<ControllerContext>();

        public ControllerBuilder WithMapper(Func<RedirectRule, RedirectRuleDto> mapper)
        {
            var mock = new Mock<IRedirectRuleMapper>();

            mock.Setup(ruleMapper => ruleMapper.ModelToDto(It.IsAny<RedirectRule>())).Returns(mapper);

            _redirectRuleMapper = mock.Object;
            return this;
        }
        
        public ControllerBuilder WithHttpResponseHeaders()
        { 
            var httpContext = new HttpContextBuilder()
                .WithRequest()
                .WithResponse()
                .WithResponseHeaders()
                .Build();

            _controllerContext.Object.HttpContext = httpContext;

            return this;
        }

        public override RedirectRuleStore Create()
        {
            CreateRepository();
           
            var controller = new RedirectRuleStore(RedirectRuleRepository, _redirectRuleMapper)
            {
                ControllerContext = _controllerContext.Object
            };
            return controller;
        }


       
    }
}
