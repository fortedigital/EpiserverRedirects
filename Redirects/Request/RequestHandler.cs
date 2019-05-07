using System.Threading.Tasks;
using Forte.RedirectMiddleware.Model.RedirectResult;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Resolver;

namespace Forte.RedirectMiddleware.Request
{
    //[ServiceConfiguration(Lifecycle = ServiceInstanceScope.Hybrid)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RequestHandler
    {
        private readonly IRedirectRuleResolver _redirectRuleResolver;
        private readonly IResponseStatusCodeResolver _responseStatusCodeResolver;

        public RequestHandler(IRedirectRuleResolver redirectRuleResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            _redirectRuleResolver = redirectRuleResolver;
            _responseStatusCodeResolver = responseStatusCodeResolver;
        }

        public async Task Invoke(ContextAdapter.ContextAdapter contextAdapter)
        {
            var requestPath = UrlPath.FromUri(contextAdapter.OldUri);
            
            var redirectRule = await _redirectRuleResolver.ResolveRedirectRule(requestPath);

            if (redirectRule != null)
                RedirectResponse(contextAdapter, redirectRule.ToRedirectResult());
        }
            
        private void RedirectResponse(ContextAdapter.ContextAdapter contextAdapter, RedirectResult redirectResult)
        {
            contextAdapter.StatusCode = _responseStatusCodeResolver.GetHttpResponseStatusCode(redirectResult);
            contextAdapter.Location = redirectResult.NewUrl;
            contextAdapter.Redirect();
        }
    }
}