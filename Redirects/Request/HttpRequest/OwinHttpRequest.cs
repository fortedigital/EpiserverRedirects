using System;
using Microsoft.Owin;

namespace Forte.RedirectMiddleware.Request.HttpRequest
{
    public class OwinHttpRequest : IHttpRequest
    {
        public Uri Url => _owinContext.Request.Uri;
        
        private readonly IOwinContext _owinContext;
        public OwinHttpRequest(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

    }
}