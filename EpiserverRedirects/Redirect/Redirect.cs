using System;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Request;
using Forte.EpiserverRedirects.Resolver;

namespace Forte.EpiserverRedirects.Redirect
{
    public abstract class Redirect : IRedirect
    {
        public Identity Id => RedirectRule.Id;
        public int Priority => RedirectRule.Priority;

        protected RedirectRule RedirectRule { get; }

        protected Redirect(RedirectRule redirectRule)
        {
            RedirectRule = redirectRule;
        }

        protected abstract string GetPathWithoutContentId(Uri request);
        
        public void Execute(Uri request, IHttpResponse response, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            var newUrl = RedirectRule.ContentId != null
                ? GetPathFromContentId(contentUrlResolver, request)
                    : GetPathWithoutContentId(request);

            RedirectResponse(response, responseStatusCodeResolver, newUrl);
        }

        private string GetPathFromContentId(IUrlResolver contentUrlResolver, Uri request)
        {
            var contentReference = new ContentReference(RedirectRule.ContentId.Value);
            var newUrl = contentUrlResolver.GetUrl(contentReference, null);
            return Configuration.Configuration.PreserveQueryString ? newUrl + request.Query : newUrl;
        }

        private void RedirectResponse(IHttpResponse httpResponse, IResponseStatusCodeResolver responseStatusCodeResolver, string newUrl)
        {
            var location = newUrl;
            var statusCode = responseStatusCodeResolver.GetHttpResponseStatusCode(RedirectRule.RedirectType);

            httpResponse.Redirect(location, statusCode);
        }

    }
}