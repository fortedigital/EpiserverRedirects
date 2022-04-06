using System;
using System.Web.Mvc;
using Forte.EpiserverRedirects.Tests.Mapper;
using Moq;

namespace Forte.EpiserverRedirects.Tests.Builder.WithRepository
{
    public class ControllerBuilder : BaseWithRepositoryBuilder<RedirectRuleStore, ControllerBuilder>
    {
        protected override ControllerBuilder ThisBuilder => this;

        private IRedirectRuleMapper _redirectRuleMapper = new RedirectRuleMapper();
        private readonly Mock<ControllerContext> _controllerContext = new Mock<ControllerContext>();

        public ControllerBuilder WithMapper(Func<RedirectRule, RedirectRuleDto> mapper)
        {
            _redirectRuleMapper = new RedirectRuleTestMapper(mapper);
            return this;
        }
        
        public ControllerBuilder WithHttpResponseHeaders()
        { 
            var httpContext = new HttpContextBuilder()
                .WithRequest()
                .WithResponse()
                .WithResponseHeaders()
                .Build();
            _controllerContext.Setup(cc => cc.HttpContext).Returns(httpContext);

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
