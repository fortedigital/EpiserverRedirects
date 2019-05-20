using System;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.Redirects.Model;
using Forte.Redirects.Request;

namespace Forte.Redirects.Redirect
{
    public interface IRedirect
    {
        Identity Id { get; }

        void Execute(Uri request, IHttpResponse httpResponse, IUrlResolver contentUrlResolver,
            IResponseStatusCodeResolver responseStatusCodeResolver);
    }
}