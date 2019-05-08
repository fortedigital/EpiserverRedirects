using System;

namespace Forte.RedirectMiddleware.Request.HttpContext
{
    public class HttpModuleHttpContext : IHttpContext
    {
        public Uri RequestUri => _httpContext.Request.Url;
        
        private readonly System.Web.HttpContext _httpContext;
        public HttpModuleHttpContext(System.Web.HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        
        public void ResponseRedirect(string location, int statusCode)
        { 
            _httpContext.Response.RedirectLocation = location; 
            _httpContext.Response.StatusCode = statusCode;
        }
    }
}