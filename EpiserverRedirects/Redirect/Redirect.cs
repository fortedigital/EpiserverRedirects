using System;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Request;

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

        protected abstract string GetPathWithoutContentId(Uri request, bool shouldPreserveQueryString);
        
        public void Execute(Uri requestUri, IRedirectHttpResponse response, IUrlResolver contentUrlResolver, bool shouldPreserveQueryString)
        {
            var newUrl = RedirectRule.ContentId != null
                ? GetPathFromContentId(contentUrlResolver, requestUri, shouldPreserveQueryString)
                    : GetPathWithoutContentId(requestUri, shouldPreserveQueryString);

            response.Redirect(newUrl, RedirectRule.RedirectType);
        }

        private string GetPathFromContentId(IUrlResolver contentUrlResolver, Uri request, bool shouldPreserveQueryString)
        {
            var contentReference = new ContentReference(RedirectRule.ContentId.Value);
            var newUrl = contentUrlResolver.GetUrl(contentReference, null);
            return shouldPreserveQueryString ? newUrl + request.Query : newUrl;
        }
    }
}