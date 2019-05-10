using EPiServer.Core;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Result;
using Moq;
using Xunit;

namespace RedirectTests.Tests.RedirectResult
{
    public class RedirectResultTests
    {
        private static IUrlResolver UrlResolver()
        {
            Mock<IUrlResolver> urlResolver = new Mock<IUrlResolver>();
            urlResolver.Setup(ur => ur.GetUrl(It.IsAny<ContentReference>(),
                It.IsAny<string>(),
                It.IsAny<UrlResolverArguments>())).Returns("/newContentUrl");
            return urlResolver.Object;
        }
        
        [Fact]
        public async void Given_ContentIdRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var redirectRule = new RedirectRule{ContentId = 35};

            var redirectResult = redirectRule.ToRedirectResult("/requestPath", UrlResolver());

            Assert.Equal("/newContentUrl", redirectResult.NewUrl);
        }
        
        [Fact]
        public async void Given_ExactMatchRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var redirectRule = new RedirectRule
            {
                RedirectRuleType = RedirectRuleType.ExactMatch,
                NewPattern = "newUrl"
            };

            var redirectResult = redirectRule.ToRedirectResult("/requestPath", UrlResolver());

            Assert.Equal(redirectRule.NewPattern, redirectResult.NewUrl);
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

            var redirectResult = redirectRule.ToRedirectResult("/requestPath/oldPattern", UrlResolver());

            Assert.Equal("/requestPath/newPattern/oldPattern", redirectResult.NewUrl);
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

            var redirectResult = redirectRule.ToRedirectResult("/requestPath/oldPattern", UrlResolver());

            Assert.Equal("newPattern/oldPattern", redirectResult.NewUrl);
        }
    }
}