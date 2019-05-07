using System.Runtime.CompilerServices;

namespace Forte.RedirectMiddleware.Model.RedirectResult
{
    public class RedirectResult
    {
        public string NewUrl { get; set; }
        public RedirectType.RedirectType RedirectType { get; set; }
    }
    
    public static class RedirectRuleExtensions
    {
        public static RedirectResult ToRedirectResult(this RedirectRule.RedirectRule redirectRule)
        {
            if (redirectRule == null)
                return null;
            
            return new RedirectResult
            {
                NewUrl = redirectRule.NewPattern,
                RedirectType = redirectRule.RedirectType
            };
        }
    }
}