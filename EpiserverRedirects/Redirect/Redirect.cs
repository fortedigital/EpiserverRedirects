using EPiServer.Core;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Request;
using System;


namespace Forte.EpiserverRedirects.Redirect
{
    public abstract class Redirect : IRedirect
    {
        public Guid? Id => RedirectRule.Id;

        public int Priority => RedirectRule.Priority;

        protected RedirectRuleModel RedirectRule { get; }

        protected Redirect(RedirectRuleModel redirectRule)
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