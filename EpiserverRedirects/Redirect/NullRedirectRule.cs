using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Data;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Request;
using Forte.EpiserverRedirects.Resolver;

namespace Forte.EpiserverRedirects.Redirect
{
    class NullRedirectRule: IRedirect
    {
        public Identity Id => null;
        public int Priority => int.MaxValue;

        public void Execute(Uri request, IHttpResponse httpResponse, IUrlResolver contentUrlResolver,
            IResponseStatusCodeResolver responseStatusCodeResolver)
        {
            // Null object pattern
        }
    }
}
