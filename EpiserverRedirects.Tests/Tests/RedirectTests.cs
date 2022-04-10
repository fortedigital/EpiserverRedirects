using Forte.EpiserverRedirects.Tests.Builder.Redirect;
using Moq;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Tests
{
    public class RedirectTests
    {
        private static RedirectBuilder Redirect() => new RedirectBuilder();

        [Fact]
        public async void Given_ContentIdRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var contentRedirect = Redirect()
                .WithContentRedirectRule(out var redirectRule)
                .WithHttpRequest(out var httpRequest, "/requestPath")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            contentRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, false);

            httpResponseMock.Verify(r => r.Redirect("/newContentUrl", redirectRule.RedirectType), Times.Once);
        }
        
        [Fact]
        public async void Given_ExactMatchRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var exactMatchRedirect = Redirect()
                .WithExactMatchRedirectRule(out var redirectRule, "newUrl")
                .WithHttpRequest(out var httpRequest, "/requestPath")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();

            exactMatchRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, false);

            httpResponseMock.Verify(r => r.Redirect(redirectRule.NewPattern, redirectRule.RedirectType), Times.Once);
        }
        
        [Fact]
        public async void Given_RegexRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var regexRedirect = Redirect()
                .WithRegexRedirectRule(out var redirectRule, "(oldPattern)", "newPattern/$1")
                .WithHttpRequest(out var httpRequest, "/requestPath/oldPattern")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            regexRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, false);

            httpResponseMock.Verify(r => r.Redirect("/requestPath/newPattern/oldPattern", redirectRule.RedirectType), Times.Once);
        }
    }
}
