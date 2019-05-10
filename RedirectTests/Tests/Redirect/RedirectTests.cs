using System;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Redirect.ExactMatch;
using Forte.RedirectMiddleware.Redirect.Regex;
using Forte.RedirectMiddleware.Redirect.Wildcard;
using Forte.RedirectMiddleware.Request.HttpContext;
using Moq;
using Xunit;

namespace RedirectTests.Tests.Redirect
{
    public class RedirectTests
    {
        private static string NewUrl { get; set; }
        private static IUrlResolver UrlResolver()
        {
            Mock<IUrlResolver> urlResolver = new Mock<IUrlResolver>();
            urlResolver.Setup(ur => ur.GetUrl(It.IsAny<ContentReference>(),
                It.IsAny<string>(),
                It.IsAny<UrlResolverArguments>())).Returns("/newContentUrl");
            return urlResolver.Object;
        }
        
        private static IHttpContext HttpContext(string requestUrl)
        {
            var httpContext = new Mock<IHttpContext>();
            httpContext.Setup(context => context.RequestUri).Returns(new Uri(requestUrl, UriKind.Relative));
            httpContext.Setup(context => context
                .ResponseRedirect(It.IsAny<string>(), It.IsAny<int>()))
                .Callback<string, int>((location, statusCode)=>NewUrl = location);
            return httpContext.Object;
        }

        private static IResponseStatusCodeResolver ResponseStatusCodeResolver => new Http_1_1_ResponseStatusCodeResolver();
        
        [Fact]
        public async void Given_ContentIdRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var redirectRule = new RedirectRule{ContentId = 35};
            var redirectRuleExecutor = new ExactMatchRedirect(redirectRule);
            var httpContext = HttpContext("/requestPath");
            
            redirectRuleExecutor.Execute(httpContext, UrlResolver(), ResponseStatusCodeResolver);

            Assert.Equal("/newContentUrl", NewUrl);
        }
        
        [Fact]
        public async void Given_ExactMatchRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var redirectRule = new RedirectRule
            {
                RedirectRuleType = RedirectRuleType.ExactMatch,
                NewPattern = "newUrl"
            };
            var exactMatchRedirect = new ExactMatchRedirect(redirectRule);
            var httpContext = HttpContext("/requestPath");

            exactMatchRedirect.Execute(httpContext, UrlResolver(), ResponseStatusCodeResolver);

            Assert.Equal(redirectRule.NewPattern, NewUrl);
        }
        
        [Fact]
        public async void Given_RegexRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var redirectRule = new RedirectRule
            {
                RedirectRuleType = RedirectRuleType.Regex,
                OldPattern = "(oldPattern)",
                NewPattern = "newPattern/$1"
            };
            
            var regexRedirect = new RegexRedirect(redirectRule);
            var httpContext = HttpContext("/requestPath/oldPattern");

            regexRedirect.Execute(httpContext, UrlResolver(), ResponseStatusCodeResolver);

            Assert.Equal("/requestPath/newPattern/oldPattern", NewUrl);
        }
        
        [Fact]
        public async void Given_WildcardRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var redirectRule = new RedirectRule
            {
                RedirectRuleType = RedirectRuleType.Wildcard,
                OldPattern = "*oldPattern*",
                NewPattern = "newPattern/{1}"
            };

            var wildcardRedirect = new WildcardRedirect(redirectRule);
            var httpContext = HttpContext("/requestPath/oldPattern");

            wildcardRedirect.Execute(httpContext, UrlResolver(), ResponseStatusCodeResolver);

            throw new NotImplementedException();
            //Assert.Equal("/requestPath/newPattern/oldPattern", NewUrl);
        }
    }
}