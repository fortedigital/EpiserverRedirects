using System;
using EPiServer.Web.Routing;
using Forte.Redirects.Model.RedirectRule;

namespace Forte.Redirects.Redirect
{
    public class WildcardRedirect : Redirect
    {
        public WildcardRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request)
        {
            var newUrl = "   ";

            return newUrl;
        }
    }
}