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
                var requestUrl = context.Request.Uri.AbsolutePath.NormalizePath();
                var urlRewriteModel = RedirectHelper.GetRedirectModel(requestUrl);

                if (urlRewriteModel != null)
                {
                    var redirectUrl = GetRedirectUrl(context, urlRewriteModel, requestUrl);

                    if (IsContentDeleted(urlRewriteModel.ContentId))
                    {
                        return;
                    }                    
                    
                    context.Response.StatusCode = (int)urlRewriteModel.RedirectStatusCode;
                    context.Response.Headers.Set("Location", redirectUrl);
                }
            }
        }

        private static string GetRedirectUrl(IOwinContext context, UrlRedirectsDto urlRewriteModel, string requestUrl)
        {
            var newPath = urlRewriteModel.Type == UrlRedirectsType.System
                ? urlRewriteModel.NewUrl
                : RedirectHelper.GetRedirectUrl(requestUrl, urlRewriteModel);
            return newPath + context.Request.Uri.Query;
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