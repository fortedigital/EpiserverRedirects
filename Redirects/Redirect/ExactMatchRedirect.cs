using System;
using EPiServer.Web.Routing;
using Forte.Redirects.Model.RedirectRule;

namespace Forte.Redirects.Redirect
{
    public class ExactMatchRedirect : Redirect
    {
        public ExactMatchRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request)
        {
            var newUrl = RedirectRule.NewPattern;

            return newUrl;
        }
    }
}