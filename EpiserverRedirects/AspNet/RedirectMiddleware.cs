using System;
using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Forte.EpiserverRedirects.Request;
using Microsoft.Owin;

namespace Forte.EpiserverRedirects.AspNet
{
    public class RedirectMiddleware : OwinMiddleware
    {
        private const int NotFoundStatusCode = 404;
        
        private readonly Func<RequestHandler> _requestHandlerFactory;

        public RedirectMiddleware(OwinMiddleware next, Func<RequestHandler> requestHandlerFactory) : base(next)
        {
            _requestHandlerFactory = requestHandlerFactory;
        }

        public RedirectMiddleware(OwinMiddleware next) : this(next, () => ServiceLocator.Current.GetInstance<RequestHandler>())
        {          
        }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (context.Response.StatusCode == NotFoundStatusCode)
            {
                var handler = _requestHandlerFactory();
                var request = context.Request.Uri;
                var response = new OwinHttpResponse(context.Response);
                await handler.Invoke(request, response);
            }
        }
    }
}