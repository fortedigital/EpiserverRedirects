using System.Text.RegularExpressions;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectResult;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Result
{
    public static class RedirectRuleExtensions
    {
        public static RedirectResult ToRedirectResult(this RedirectRule redirectRule, string requestPath, IUrlResolver contentUrlResolver)
        {
            if (redirectRule == null)
                return null;
            
            var newUrl = redirectRule.ContentId != null 
                ? GetPathFromContentId(redirectRule.ContentId.Value, contentUrlResolver)
                : GetPathWithoutContentId(redirectRule, requestPath);
            
            return new RedirectResult
            {
                NewUrl = newUrl,
                RedirectType = redirectRule.RedirectType
            };
        }

        private static string GetPathFromContentId(int contentReferenceId, IUrlResolver contentUrlResolver)
        {
            var contentReference = new ContentReference(contentReferenceId);
            return contentUrlResolver.GetUrl(contentReference, null);
        }
        
        private static string GetPathWithoutContentId(RedirectRule redirectRule, string requestPath)
        {
            string newUrl = null;
            switch (redirectRule.RedirectRuleType)
            {
                case RedirectRuleType.ExactMatch:
                    newUrl = redirectRule.NewPattern;
                    break;
                case RedirectRuleType.Regex:
                    newUrl = Regex.Replace(requestPath, redirectRule.OldPattern,
                        redirectRule.NewPattern, RegexOptions.IgnoreCase);
                    break;
                case RedirectRuleType.Wildcard:
                    break;
            }

            return newUrl;
        }
    }
}