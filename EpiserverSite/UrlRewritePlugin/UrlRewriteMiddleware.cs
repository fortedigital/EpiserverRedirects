using Microsoft.Owin;
using System.Threading.Tasks;

namespace EpiserverSite.UrlRewritePlugin
{
    public class UrlRewriteMiddleware : OwinMiddleware
    {
        public UrlRewriteMiddleware(OwinMiddleware next) : base(next) { }

        public async override Task Invoke(IOwinContext context)
        {
            var url = context.Request.Path.ToString().NormalizePath();
            var urlRewriteModel = RedirectHelper.GetRedirectModel(url);

            if (urlRewriteModel != null)
            {
                var redirectUrl = RedirectHelper.GetRedirectUrl(urlRewriteModel.ContextId);

                if (string.IsNullOrEmpty(redirectUrl))
                {
                    context.Response.StatusCode = 404;
                } 
                else
                {
                    context.Response.Redirect(redirectUrl);
                }
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}