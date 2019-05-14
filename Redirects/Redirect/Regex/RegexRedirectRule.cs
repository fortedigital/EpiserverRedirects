using System;
using System.Text.RegularExpressions;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Request;

namespace Forte.RedirectMiddleware.Redirect.Regex
{
    public class RegexRedirect : Redirect.Base.Redirect
    {
        public RegexRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }
        
        protected override string GetPathWithoutContentId(Uri request, IUrlResolver contentUrlResolver,
            IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            var newUrl = System.Text.RegularExpressions.Regex.Replace(request.ToString(), RedirectRule.OldPattern,
                RedirectRule.NewPattern, RegexOptions.IgnoreCase);
            return newUrl;
        }
    }
}