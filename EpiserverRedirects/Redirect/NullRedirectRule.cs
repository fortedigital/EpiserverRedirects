using System;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Request;

namespace Forte.EpiserverRedirects.Redirect
{
    class NullRedirectRule: IRedirect
    {
        public Guid? Id => null;
        public int Priority => int.MaxValue;

        public void Execute(Uri requestUri, IRedirectHttpResponse redirectHttpResponse, IUrlResolver contentUrlResolver, bool shouldPreserveQueryString)
        {
            // Null object pattern
        }
    }
}
