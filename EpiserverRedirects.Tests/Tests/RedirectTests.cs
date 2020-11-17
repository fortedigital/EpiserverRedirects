using System;
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
                .WithHttp_1_1_ResponseStatusCodeResolver(out var statusCodeResolver)
                .WithHttpRequest(out var httpRequest, "/requestPath")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            contentRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, statusCodeResolver);
            
            httpResponseMock.Verify(r => r.Redirect("/newContentUrl",
                    statusCodeResolver.GetHttpResponseStatusCode(redirectRule.RedirectType)),
                Times.Once);
        }
        
        [Fact]
        public async void Given_ExactMatchRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var exactMatchRedirect = Redirect()
                .WithExactMatchRedirectRule(out var redirectRule, "newUrl")
                .WithHttp_1_1_ResponseStatusCodeResolver(out var statusCodeResolver)
                .WithHttpRequest(out var httpRequest, "/requestPath")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();

            exactMatchRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, statusCodeResolver);

            httpResponseMock.Verify(r => r.Redirect(redirectRule.NewPattern,
                    statusCodeResolver.GetHttpResponseStatusCode(redirectRule.RedirectType)),
                Times.Once);
        }
        
        [Fact]
        public async void Given_RegexRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var regexRedirect = Redirect()
                .WithRegexRedirectRule(out var redirectRule, "(oldPattern)", "newPattern/$1")
                .WithHttp_1_1_ResponseStatusCodeResolver(out var statusCodeResolver)
                .WithHttpRequest(out var httpRequest, "/requestPath/oldPattern")
                .WithHttpResponseMock(out var httpResponseMock)
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            regexRedirect.Execute(httpRequest, httpResponseMock.Object, urlResolver, statusCodeResolver);

            httpResponseMock.Verify(r => r.Redirect("/requestPath/newPattern/oldPattern",
                    statusCodeResolver.GetHttpResponseStatusCode(redirectRule.RedirectType)),
                Times.Once);
        }
    }
}
