using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Service;
using Microsoft.Owin;

namespace Forte.RedirectMiddleware
{
    public class RedirectMiddleware : OwinMiddleware
    {
        private const int NotFoundStatusCode = 404;
        private const string LocationHeader = "Location";
        private Injected<IRedirectService> RedirectService  { get; set; }
        public RedirectMiddleware(OwinMiddleware next) : base(next) {}

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (context.Response.StatusCode == NotFoundStatusCode)
            {
                var originalRequestPath = context.Request.Uri.AbsolutePath;
                var redirectModel = RedirectService.Service.GetRedirect(originalRequestPath);

                if (redirectModel != null)
                    RedirectResponse(context, redirectModel);
            }
        }

        private static void RedirectResponse(IOwinContext context, RedirectModel redirectModel)
        {
            context.Response.StatusCode = (int) redirectModel.StatusCode;
            context.Response.Headers.Set(LocationHeader, redirectModel.NewUrl);
        }
    }
}