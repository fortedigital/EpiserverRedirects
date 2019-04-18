using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Repository;
using Forte.RedirectMiddleware.Service;
using Microsoft.Owin;

namespace Forte.RedirectMiddleware
{
    public class RedirectMiddleware : OwinMiddleware
    {
        private const int NotFoundStatusCode = 404;
        private const string LocationHeader = "Location";
        private IRedirectRuleResolver RedirectService  { get; set; }
        private IResponseStatusCodeResolver ResponseStatusCodeResolver  { get; set; }

        public RedirectMiddleware(OwinMiddleware next, IRedirectRuleResolver redirectService, IResponseStatusCodeResolver responseStatusCodeResolver) : base(next)
        {
            RedirectService = redirectService;
            ResponseStatusCodeResolver = responseStatusCodeResolver;
        }

        public RedirectMiddleware(OwinMiddleware next) : this(next,
            ServiceLocator.Current.GetInstance<IRedirectRuleResolver>(),
            ServiceLocator.Current.GetInstance<IResponseStatusCodeResolver>()){          
        }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (context.Response.StatusCode == NotFoundStatusCode)
            {
                var originalRequestPath = context.Request.Uri.AbsolutePath;
                var redirectRule = RedirectService.ResolveRedirectRule(UrlPath.Create(originalRequestPath));

                if (redirectRule != null)
                    RedirectResponse(context, redirectRule);
            }
        }

        private void RedirectResponse(IOwinContext context, RedirectRule redirectRule)
        {
            context.Response.StatusCode = ResponseStatusCodeResolver.GetHttpResponseStatusCode(redirectRule);
            context.Response.Headers.Set(LocationHeader, redirectRule.NewUrl);
        }
    }
}