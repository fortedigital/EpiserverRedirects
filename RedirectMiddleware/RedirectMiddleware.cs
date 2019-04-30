using System;
using System.Threading.Tasks;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using EPiServer.Web.Internal;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Model.UrlPath;
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
                
                try
                {
                    if(!GlobalTypeHandlers.Instance.ContainsKey(typeof(UrlPath)))
                        GlobalTypeHandlers.Instance.Add(typeof(UrlPath), new UrlPathTypeHandler());
                    
                    //if(!GlobalTypeHandlers.Instance.ContainsKey(typeof(Uri)))
                    //    GlobalTypeHandlers.Instance.Add(typeof(Uri), new CustomUriTypeHandler());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
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