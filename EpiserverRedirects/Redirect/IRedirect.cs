using System;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Request;

namespace Forte.EpiserverRedirects.Redirect
{
    public interface IRedirect
    {
        Identity Id { get; }
        int Priority { get; }

        void Execute(Uri requestUri, IRedirectHttpResponse redirectHttpResponse, IUrlResolver contentUrlResolver, bool shouldPreserveQueryString);
    }
}