using System.Threading.Tasks;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Request.HttpRequest;
using Forte.RedirectMiddleware.Resolver.Base;
using Forte.RedirectMiddleware.Response.HttpResponse;

namespace Forte.RedirectMiddleware.Request
{
    public class RequestHandler
    {
        private readonly IRedirectRuleResolver _redirectRuleResolver;
        private readonly IResponseStatusCodeResolver _responseStatusCodeResolver;
        private readonly IUrlResolver _urlResolver;

        public RequestHandler(IRedirectRuleResolver redirectRuleResolver,
            IResponseStatusCodeResolver responseStatusCodeResolver,
            IUrlResolver urlResolver)
        {
            _redirectRuleResolver = redirectRuleResolver;
            _responseStatusCodeResolver = responseStatusCodeResolver;
            _urlResolver = urlResolver;
        }

        public async Task Invoke(IHttpRequest request, IHttpResponse response)
        {
            var requestPath = UrlPath.FromUri(request.Url);
            
            var redirectRule = await _redirectRuleResolver.ResolveRedirectRule(requestPath);

            redirectRule?.Execute(request, response, _urlResolver, _responseStatusCodeResolver);
        }
    }
}