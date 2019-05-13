using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Request.HttpRequest;
using Forte.RedirectMiddleware.Response.HttpResponse;

namespace Forte.RedirectMiddleware.Redirect.Base
{
    public interface IRedirect
    {
        Identity Id { get; }
        void Execute(IHttpRequest request, IHttpResponse httpResponse, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver);
    }
}