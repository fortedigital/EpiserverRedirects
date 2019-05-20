using System;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.Redirects.Model;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Request;

namespace Forte.Redirects.Redirect
{
    public abstract class Redirect : IRedirect
    {
        public Identity Id => RedirectRule.Id;
        protected RedirectRule RedirectRule { get; }

        protected Redirect(RedirectRule redirectRule)
        {
            RedirectRule = redirectRule;
        }

        protected abstract string GetPathWithoutContentId(Uri request);
        
        public void Execute(Uri request, IHttpResponse response, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            var newUrl = RedirectRule.ContentId != null
                ? GetPathFromContentId(contentUrlResolver)
                    : GetPathWithoutContentId(request);
            
            RedirectResponse(response, responseStatusCodeResolver, newUrl);
        }

        private string GetPathFromContentId(IUrlResolver contentUrlResolver)
        {
            var contentReference = new ContentReference(RedirectRule.ContentId.Value);
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