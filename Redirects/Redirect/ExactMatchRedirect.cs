using System;
using EPiServer.Web.Routing;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.RedirectType;

namespace Forte.Redirects.Redirect
{
    public class ExactMatchRedirect : Redirect
    {
        public ExactMatchRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            var newUrl = RedirectRule.NewPattern;

            return newUrl;
        }
    }
}