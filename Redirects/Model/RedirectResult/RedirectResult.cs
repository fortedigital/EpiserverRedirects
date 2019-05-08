using System.Runtime.CompilerServices;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace Forte.RedirectMiddleware.Model.RedirectResult
{
    public class RedirectResult
    {
        public string NewUrl { get; set; }
        public RedirectType.RedirectType RedirectType { get; set; }
    }
    
    public static class RedirectRuleExtensions
    {
        public static RedirectResult ToRedirectResult(this RedirectRule.RedirectRule redirectRule, IUrlResolver contentUrlResolver)
        {
            if (redirectRule == null)
                return null;

            string newUrl;

            if (redirectRule.ContentId == null)
                newUrl = redirectRule.NewPattern;

            else
            {
                var contentReference = new ContentReference(redirectRule.ContentId.Value);
                var virtualPathData = contentUrlResolver.GetUrl(contentReference, null);

                newUrl = virtualPathData;
            }

            return new RedirectResult
            {
                NewUrl = newUrl,
                RedirectType = redirectRule.RedirectType
            };
        }
    }
}