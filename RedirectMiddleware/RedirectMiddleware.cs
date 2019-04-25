using System;
using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Repository;
using Forte.RedirectMiddleware.Resolver;
using Microsoft.Owin;

namespace Forte.RedirectMiddleware
{
    public class RedirectMiddleware : OwinMiddleware
    {
        private const int NotFoundStatusCode = 404;
        private const string LocationHeader = "Location";
        private IRedirectRuleResolver RedirectRuleResolver  { get; }
        private IResponseStatusCodeResolver ResponseStatusCodeResolver  { get; }

        public RedirectMiddleware(OwinMiddleware next, IRedirectRuleResolver redirectRuleResolver, IResponseStatusCodeResolver responseStatusCodeResolver) : base(next)
        {
            RedirectRuleResolver = redirectRuleResolver;
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
                var requestPath = UrlPath.FromUri(context.Request.Uri);
                var redirectRule = RedirectRuleResolver.ResolveRedirectRule(requestPath);

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