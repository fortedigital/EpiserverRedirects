using System;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;
using Forte.EpiserverRedirects.Request;
using Moq;

namespace Forte.EpiserverRedirects.Tests.Builder.Redirect
{
    public class RedirectBuilder
    {
        private IRedirect _redirect;
        private readonly RedirectRule _redirectRule = new RedirectRule();
        internal RedirectBuilder() { }

        public RedirectBuilder WithHttpRequest(out Uri request, string requestPath)
        {
            request = HttpRequest(requestPath);
            return this;
        }
        
        public RedirectBuilder WithHttpResponseMock(out Mock<IRedirectHttpResponse> httpResponseMock)
        {
            httpResponseMock = new Mock<IRedirectHttpResponse>();
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

        private static Uri HttpRequest(string requestUrl) => new Uri(requestUrl, UriKind.Relative);

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
        
        public IRedirect Create() => _redirect;
    }
}