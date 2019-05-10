using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Request.HttpContext;

namespace Forte.RedirectMiddleware.Redirect.Base
{
    public interface IRedirect
    {
        RedirectRule RedirectRule { get; }
        void Execute(IHttpContext context, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver);
    }
}