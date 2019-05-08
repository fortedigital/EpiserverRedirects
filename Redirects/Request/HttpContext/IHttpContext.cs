using System;

namespace Forte.RedirectMiddleware.Request.HttpContext
{
    public interface IHttpContext
    {
        Uri RequestUri { get; }
        
        void ResponseRedirect(string location, int statusCode);
    }
}