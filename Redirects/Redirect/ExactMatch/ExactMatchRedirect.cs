using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Request.HttpContext;

namespace Forte.RedirectMiddleware.Redirect.ExactMatch
{
    public class ExactMatchRedirect : Redirect.Base.Redirect
    {
        public ExactMatchRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(IHttpContext context, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            var newUrl = RedirectRule.NewPattern;

            return newUrl;
        }
    }
}