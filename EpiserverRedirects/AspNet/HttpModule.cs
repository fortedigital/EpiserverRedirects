using System;
using System.Web;
using EPiServer.ServiceLocation;
using Forte.EpiserverRedirects.Request;

namespace Forte.EpiserverRedirects.AspNet
{
    public class HttpModule : IHttpModule {
        
        private const int NotFoundStatusCode = 404;
        
        private readonly Func<RequestHandler> _requestHandlerFactory;

        public HttpModule()
        {
            _requestHandlerFactory = () => ServiceLocator.Current.GetInstance<RequestHandler>();
        }
        
        public void Init(HttpApplication context)
        {
            context.EndRequest += RedirectRequest;
        }

        private async void RedirectRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication) sender;
            var context = app.Context;
            if (context.Response.StatusCode == NotFoundStatusCode)
            {
                var handler = _requestHandlerFactory();
                var request = context.Request.Url;
                var response = new HttpModuleHttpResponse(context.Response);
                await handler.Invoke(request, response);
            }
        }


        public void Dispose()
        {
            throw new global::System.NotImplementedException();
        }
    }
}