using System;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Request;

namespace Forte.RedirectMiddleware.Redirect.Base
{
    public interface IRedirect
    {
        Identity Id { get; }
        void Execute(Uri request, IHttpResponse httpResponse, IUrlResolver contentUrlResolver, IResponseStatusCodeResolver responseStatusCodeResolver);
    }
}