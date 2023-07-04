using Forte.EpiserverRedirects.Tests.Builder.Redirect;
using Moq;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Tests
{
    public class RedirectTests
    {
        private static RedirectBuilder Redirect() => new RedirectBuilder();

        [Fact]
        public void Given_ContentIdRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var contentRedirect = Redirect()
                .WithContentRedirectRule(out var redirectRule)
                .WithRelativeHttpRequest(out var httpRequest, "/requestPath")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            contentRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, false);

            httpResponseMock.Verify(r => r.Redirect("/newContentUrl", redirectRule.RedirectType), Times.Once);
        }
        
        [Fact]
        public void Given_ExactMatchRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var exactMatchRedirect = Redirect()
                .WithExactMatchRedirectRule(out var redirectRule, "newUrl")
                .WithRelativeHttpRequest(out var httpRequest, "/requestPath")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();

            exactMatchRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, false);

            httpResponseMock.Verify(r => r.Redirect(redirectRule.NewPattern, redirectRule.RedirectType), Times.Once);
        }
        
        [Fact]
        public void Given_RegexRedirectRule_WhenNewPatternRelative_ReturnsCorrectResultInTheSameHost()
        {
            const string urlBase = "https://localhost:8080";
            
            var regexRedirect = Redirect()
                .WithRegexRedirectRule(out var redirectRule, "/requestPath/(oldPattern)", "/newPattern/$1")
                .WithAbsoluteHttpRequest(out var httpRequest, $"{urlBase}/requestPath/oldPattern")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            regexRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, false);

            httpResponseMock.Verify(r => r.Redirect("/newPattern/oldPattern", redirectRule.RedirectType), Times.Once);
        }
        
        
        [Fact]
        public void Given_RegexRedirectRule_WhenNewPatternAbsolute_ReturnsCorrectResultInTheNewPatternHost()
        {
            const string urlBase = "https://localhost:8080";
            const string newUrlBase = "https://localhost:4124";
            
            var regexRedirect = Redirect()
                .WithRegexRedirectRule(out var redirectRule, "/requestPath/(oldPattern)", $"{newUrlBase}/newPattern/$1")
                .WithAbsoluteHttpRequest(out var httpRequest, $"{urlBase}/requestPath/oldPattern")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            regexRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, false);

            httpResponseMock.Verify(r => r.Redirect($"{newUrlBase}/newPattern/oldPattern", redirectRule.RedirectType), Times.Once);
        }
    }
}
