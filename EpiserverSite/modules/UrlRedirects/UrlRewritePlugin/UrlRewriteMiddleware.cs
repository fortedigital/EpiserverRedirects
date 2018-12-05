using Microsoft.Owin;
using System.Threading.Tasks;

namespace EpiserverSite.UrlRewritePlugin
{
    public class UrlRewriteMiddleware : OwinMiddleware
    {
        public UrlRewriteMiddleware(OwinMiddleware next) : base(next) { }

        public async override Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (context.Response.StatusCode == 404)
            {
                var url = context.Request.Path.ToString().NormalizePath();
                var urlRewriteModel = RedirectHelper.GetRedirectModel(url);

                if (urlRewriteModel != null)
                {
                    var redirectUrl = urlRewriteModel.Type == "system" ?
                        RedirectHelper.GetRedirectUrl(urlRewriteModel.ContentId) :
                        RedirectHelper.GetRedirectUrl(url, urlRewriteModel);

                    context.Response.StatusCode = urlRewriteModel.RedirectStatusCode;
                    context.Response.Headers.Set("Location", redirectUrl);
                }
            }

        }
    }
}