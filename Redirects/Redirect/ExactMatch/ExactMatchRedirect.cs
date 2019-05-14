using System;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Request;

namespace Forte.RedirectMiddleware.Redirect.ExactMatch
{
    public class ExactMatchRedirect : Redirect.Base.Redirect
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