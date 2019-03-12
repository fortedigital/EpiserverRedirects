using System.Linq;
using System.Threading.Tasks;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Microsoft.Owin;

namespace Forte.EpiserverRedirects.UrlRewritePlugin
{
    public class UrlRewriteMiddleware : OwinMiddleware
    {
        public UrlRewriteMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
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

                    if (IsContentDeleted(urlRewriteModel.ContentId))
                    {
                        return;
                    }                    

                    context.Response.StatusCode = (int)urlRewriteModel.RedirectStatusCode;
                    context.Response.Headers.Set("Location", redirectUrl);
                }
            }
        }

        private static bool IsContentDeleted(int contentId)
        {
            if(contentId == 0)
            {
                return false;
            }

            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return contentLoader.TryGet<IContent>(new ContentReference(contentId), out var content) == false ||
                   contentLoader.GetAncestors(content.ContentLink)
                       .Any(ancestor => ancestor.ContentLink == ContentReference.WasteBasket);
        }
    }
}