using System;
using System.Threading.Tasks;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Encoder;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Resolver;

namespace Forte.EpiserverRedirects.Request
{
    public class RequestHandler
    {
        private readonly IRedirectRuleResolver _redirectRuleResolver;
        private readonly IResponseStatusCodeResolver _responseStatusCodeResolver;
        private readonly IUrlResolver _urlResolver;
        private readonly IUrlPathEncoder _urlPathEncoder;

        public RequestHandler(IRedirectRuleResolver redirectRuleResolver,
            IResponseStatusCodeResolver responseStatusCodeResolver,
            IUrlResolver urlResolver,
            IUrlPathEncoder urlPathEncoder)
        {
            _redirectRuleResolver = redirectRuleResolver;
            _responseStatusCodeResolver = responseStatusCodeResolver;
            _urlResolver = urlResolver;
            _urlPathEncoder = urlPathEncoder;
        }

        public async Task Invoke(Uri request, IHttpResponse response)
        {
            var requestPath = UrlPath.FromUri(request);
            var requestPathEncoded = _urlPathEncoder.Encode(requestPath);
            
            var redirectRule = await _redirectRuleResolver.ResolveRedirectRuleAsync(requestPathEncoded);

            redirectRule?.Execute(request, response, _urlResolver, _responseStatusCodeResolver);
        }
    }
}