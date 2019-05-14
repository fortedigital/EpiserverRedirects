using System;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Request;

namespace Forte.RedirectMiddleware.Redirect.Base
{
    public abstract class Redirect : IRedirect
    {
        public Identity Id => RedirectRule.Id;
        protected RedirectRule RedirectRule { get; }

        protected Redirect(RedirectRule redirectRule)
        {
            RedirectRule = redirectRule;
        }

        protected abstract string GetPathWithoutContentId(Uri request, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver);
        
        public void Execute(Uri request, IHttpResponse response, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            if (GetPathFromContentId(response, contentUrlResolver, responseStatusCodeResolver))
                return;

            var newUrl = GetPathWithoutContentId(request, contentUrlResolver, responseStatusCodeResolver);

            RedirectResponse(response, responseStatusCodeResolver, newUrl);
        }

        private bool GetPathFromContentId(IHttpResponse response, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            if (RedirectRule.ContentId == null)
                return false;
            
            var newUrl = GetPathFromContentId(RedirectRule.ContentId.Value, contentUrlResolver);

            RedirectResponse(response, responseStatusCodeResolver, newUrl);

            return true;
        }

        private static string GetPathFromContentId(int contentReferenceId, IUrlResolver contentUrlResolver)
        {
            var contentReference = new ContentReference(contentReferenceId);
            return contentUrlResolver.GetUrl(contentReference, null);
        }

        private void RedirectResponse(IHttpResponse httpResponse, IResponseStatusCodeResolver responseStatusCodeResolver, string newUrl)
        {
            var location = newUrl;
            var statusCode = responseStatusCodeResolver.GetHttpResponseStatusCode(RedirectRule.RedirectType);

            httpResponse.Redirect(location, statusCode);
        }

    }
}