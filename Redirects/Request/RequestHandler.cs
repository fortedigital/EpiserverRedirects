using System.Threading.Tasks;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Request.HttpContext;
using Forte.RedirectMiddleware.Resolver.Base;

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

        public async Task Invoke(IHttpContext httpContext)
        {
            var requestPath = UrlPath.FromUri(httpContext.RequestUri);
            
            var redirectRule = await _redirectRuleResolver.ResolveRedirectRule(requestPath);

            redirectRule?.Execute(httpContext, _urlResolver, _responseStatusCodeResolver);
        }
        
    }
}