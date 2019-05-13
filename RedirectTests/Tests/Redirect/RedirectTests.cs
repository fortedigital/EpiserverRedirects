using System;
using Moq;
using RedirectTests.Builder.Redirect;
using Xunit;

namespace RedirectTests.Tests.Redirect
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
                .WithRequestPathHttpContextMoq(out var httpContextMoq, "/requestPath")
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            contentRedirect.Execute(httpContextMoq.Object, urlResolver, statusCodeResolver);
            
            httpContextMoq.Verify(c => c.ResponseRedirect("/newContentUrl",
                    statusCodeResolver.GetHttpResponseStatusCode(redirectRule.RedirectType)),
                Times.Once);
        }
        
        [Fact]
        public async void Given_ExactMatchRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var exactMatchRedirect = Redirect()
                .WithExactMatchRedirectRule(out var redirectRule, "newUrl")
                .WithHttp_1_1_ResponseStatusCodeResolver(out var statusCodeResolver)
                .WithRequestPathHttpContextMoq(out var httpContextMoq, "/requestPath")
                .WithUrlResolver(out var urlResolver)
                .Create();

            exactMatchRedirect.Execute(httpContextMoq.Object, urlResolver, statusCodeResolver);

            httpContextMoq.Verify(c => c.ResponseRedirect(redirectRule.NewPattern,
                    statusCodeResolver.GetHttpResponseStatusCode(redirectRule.RedirectType)),
                Times.Once);
        }
        
        [Fact]
        public async void Given_RegexRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var regexRedirect = Redirect()
                .WithRegexRedirectRule(out var redirectRule, "(oldPattern)", "newPattern/$1")
                .WithHttp_1_1_ResponseStatusCodeResolver(out var statusCodeResolver)
                .WithRequestPathHttpContextMoq(out var httpContextMoq, "/requestPath/oldPattern")
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            regexRedirect.Execute(httpContextMoq.Object, urlResolver, statusCodeResolver);
            
            httpContextMoq.Verify(c => c.ResponseRedirect("/requestPath/newPattern/oldPattern",
                    statusCodeResolver.GetHttpResponseStatusCode(redirectRule.RedirectType)),
                Times.Once);
        }
        
        [Fact]
        public async void Given_WildcardRedirectRule_ToRedirectResult_ReturnsCorrectResult()
        {
            var wildcardRedirect = Redirect()
                .WithWildcardRedirectRule(out var redirectRule, "*oldPattern*", "newPattern/{1}")
                .WithHttp_1_1_ResponseStatusCodeResolver(out var statusCodeResolver)
                .WithRequestPathHttpContextMoq(out var httpContextMoq, "/requestPath/oldPattern")
                .WithUrlResolver(out var urlResolver)
                .Create();
            
            wildcardRedirect.Execute(httpContextMoq.Object, urlResolver, statusCodeResolver);

            throw new NotImplementedException();
            //httpContextMoq.Verify(c => c.ResponseRedirect("/requestPath/newPattern/oldPattern",
            //        statusCodeResolver.GetHttpResponseStatusCode(redirectRule.RedirectType)),
             //   Times.Once);
        }
    }
}