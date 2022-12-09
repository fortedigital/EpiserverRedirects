using System;
using System.Threading.Tasks;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Resolver;

namespace Forte.EpiserverRedirects.Request
{
    public class RequestHandler
    {
        private readonly IRedirectRuleResolver _redirectRuleResolver;
        private readonly IUrlResolver _urlResolver;
        private readonly RedirectsOptions _options;

        public RequestHandler(IRedirectRuleResolver redirectRuleResolver, IUrlResolver urlResolver, RedirectsOptions options)
        {
            _redirectRuleResolver = redirectRuleResolver;
            _urlResolver = urlResolver;
            _options = options;
        }

        public async Task Invoke(Uri requestUri, IRedirectHttpResponse response)
        {
            var requestPath = UrlPath.FromUri(requestUri);

            var redirectRule = await _redirectRuleResolver.ResolveRedirectRuleAsync(requestPath);

            redirectRule?.Execute(requestUri, response, _urlResolver, _options.PreserveQueryString);
        }
    }
}