using System;
using System.Text.RegularExpressions;
using EPiServer.Web.Routing;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.RedirectType;

namespace Forte.Redirects.Redirect
{
    public class RegexRedirect : Redirect
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