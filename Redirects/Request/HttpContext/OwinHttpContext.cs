using System;
using Microsoft.Owin;

namespace Forte.RedirectMiddleware.Request.HttpContext
{
    public class OwinHttpContext : IHttpContext
    {
        public Uri RequestUri => this._owinContext.Request.Uri;
        
        private readonly IOwinContext _owinContext;
        public OwinHttpContext(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public void ResponseRedirect(string location, int statusCode)
        {
            _owinContext.Response.Headers.Set("Location", location);
            _owinContext.Response.StatusCode = statusCode;
        }
    }
}