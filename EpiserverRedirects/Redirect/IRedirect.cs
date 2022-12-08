using System;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Request;

namespace Forte.EpiserverRedirects.Redirect
{
    public interface IRedirect
    {
        Guid? Id { get; }
        int Priority { get; }
        void Execute(Uri requestUri, IRedirectHttpResponse redirectHttpResponse, IUrlResolver contentUrlResolver, bool shouldPreserveQueryString);
    }
}
