using System;
using EPiServer.Web.Routing;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.RedirectType;

namespace Forte.Redirects.Redirect
{
    public class WildcardRedirect : Redirect
    {
        public WildcardRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            var newUrl = "   ";

            return newUrl;
        }
    }
}