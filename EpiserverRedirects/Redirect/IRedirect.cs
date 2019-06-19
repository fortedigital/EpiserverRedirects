using System;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Request;
using Forte.EpiserverRedirects.Resolver;

namespace Forte.EpiserverRedirects.Redirect
{
    public interface IRedirect
    {
        Identity Id { get; }
        int Priority { get; }

        void Execute(Uri request, IHttpResponse httpResponse, IUrlResolver contentUrlResolver,
            IResponseStatusCodeResolver responseStatusCodeResolver);
    }
}