using EPiServer.Core;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Request.HttpContext;

namespace Forte.RedirectMiddleware.Redirect.Base
{
    public abstract class Redirect : IRedirect
    {
        public RedirectRule RedirectRule { get; }

        protected abstract string GetPathWithoutContentId(IHttpContext context, IUrlResolver contentUrlResolver,
            IResponseStatusCodeResolver responseStatusCodeResolver);

        protected Redirect(RedirectRule redirectRule)
        {
            RedirectRule = redirectRule;
        }

        public void Execute(IHttpContext context, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            if (GetPathFromContentId(context, contentUrlResolver, responseStatusCodeResolver))
                return;

            var newUrl = GetPathWithoutContentId(context, contentUrlResolver, responseStatusCodeResolver);

            RedirectResponse(context, responseStatusCodeResolver, newUrl);
        }

        private bool GetPathFromContentId(IHttpContext context, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            if (RedirectRule.ContentId == null)
                return false;
            
            var newUrl = GetPathFromContentId(RedirectRule.ContentId.Value, contentUrlResolver);

            RedirectResponse(context, responseStatusCodeResolver, newUrl);

            return true;
        }

        private static string GetPathFromContentId(int contentReferenceId, IUrlResolver contentUrlResolver)
        {
            var contentReference = new ContentReference(contentReferenceId);
            return contentUrlResolver.GetUrl(contentReference, null);
        }

        private void RedirectResponse(IHttpContext httpContext, IResponseStatusCodeResolver responseStatusCodeResolver, string newUrl)
        {
            var location = newUrl;
            var statusCode = responseStatusCodeResolver.GetHttpResponseStatusCode(RedirectRule.RedirectType);

            httpContext.ResponseRedirect(location, statusCode);
        }

    }
}