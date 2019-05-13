using System;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Redirect.Base;
using Forte.RedirectMiddleware.Redirect.ExactMatch;
using Forte.RedirectMiddleware.Redirect.Regex;
using Forte.RedirectMiddleware.Redirect.Wildcard;
using Forte.RedirectMiddleware.Request.HttpRequest;
using Forte.RedirectMiddleware.Response.HttpResponse;
using Moq;

namespace RedirectTests.Builder.Redirect
{
    public class RedirectBuilder
    {
        private IRedirect _redirect;
        private readonly RedirectRule _redirectRule = new RedirectRule();
        internal RedirectBuilder() { }

        public RedirectBuilder WithHttp_1_0_ResponseStatusCodeResolver(out IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            responseStatusCodeResolver = new Http_1_0_ResponseStatusCodeResolver();
            return this;
        }
        
        public RedirectBuilder WithHttp_1_1_ResponseStatusCodeResolver(out IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            responseStatusCodeResolver = new Http_1_1_ResponseStatusCodeResolver();
            return this;
        }
        
        public RedirectBuilder WithHttpRequest(out IHttpRequest httpContext, string requestPath)
        {
            httpContext = HttpRequest(requestPath);
            return this;
        }
        
        public RedirectBuilder WithHttpResponseMock(out Mock<IHttpResponse> httpResponseMock)
        {
            httpResponseMock = new Mock<IHttpResponse>();
            return this;
        }
        
        public RedirectBuilder WithUrlResolver(out IUrlResolver urlResolver, string newUrl = null)
        {
            var urlResolverMoq = new Mock<IUrlResolver>();
            urlResolverMoq.Setup(ur => ur.GetUrl(It.IsAny<ContentReference>(),
                It.IsAny<string>(),
                It.IsAny<UrlResolverArguments>())).Returns(newUrl ?? "/newContentUrl");
            urlResolver = urlResolverMoq.Object;
            return this;
        }

        private static IHttpRequest HttpRequest(string requestUrl)
        {
            var httpContext = new Mock<IHttpRequest>();
            httpContext.Setup(request => request.Url).Returns(new Uri(requestUrl, UriKind.Relative));
            return httpContext.Object;
        }

        public RedirectBuilder WithContentRedirectRule(out RedirectRule redirectRule, int? contentReferenceId = null)
        {
            _redirectRule.ContentId = contentReferenceId ?? new Random().Next(1, 1000);

            redirectRule = _redirectRule;
            
            _redirect = new ExactMatchRedirect(_redirectRule);
            return this;
        }

        public RedirectBuilder WithExactMatchRedirectRule(out RedirectRule redirectRule, string newPattern)
        {
            _redirectRule.RedirectRuleType = RedirectRuleType.ExactMatch;
            _redirectRule.NewPattern = newPattern;

            redirectRule = _redirectRule;

            _redirect = new ExactMatchRedirect(_redirectRule);
            return this;
        }
        
        public RedirectBuilder WithRegexRedirectRule(out RedirectRule redirectRule, string oldPattern, string newPattern)
        {
            _redirectRule.RedirectRuleType = RedirectRuleType.Regex;
            _redirectRule.OldPattern = oldPattern;
            _redirectRule.NewPattern = newPattern;

            redirectRule = _redirectRule;
            
            _redirect = new RegexRedirect(_redirectRule);
            return this;
        }
        
        public RedirectBuilder WithWildcardRedirectRule(out RedirectRule redirectRule, string oldPattern, string newPattern)
        {
            _redirectRule.RedirectRuleType = RedirectRuleType.Wildcard;
            _redirectRule.OldPattern = oldPattern;
            _redirectRule.NewPattern = newPattern;

            redirectRule = _redirectRule;

            _redirect = new WildcardRedirect(_redirectRule);
            return this;
        }
        
        public IRedirect Create() => _redirect;
    }
}