using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Request.HttpContext;

namespace Forte.RedirectMiddleware.Redirect.Base
{
    public interface IRedirect
    {
        Identity Id { get; }
        void Execute(IHttpContext context, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver);
    }
}