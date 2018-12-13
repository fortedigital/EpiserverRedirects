using System.Threading.Tasks;
using Microsoft.Owin;

namespace Forte.EpiserverRedirects.UrlRewritePlugin
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
                    var redirectUrl = urlRewriteModel.Type == UrlRedirectsType.System ?
                        urlRewriteModel.NewUrl :
                        RedirectHelper.GetRedirectUrl(url, urlRewriteModel);

                    context.Response.StatusCode = (int)urlRewriteModel.RedirectStatusCode;
                    context.Response.Headers.Set("Location", redirectUrl);
                }
            }

        }
    }
}