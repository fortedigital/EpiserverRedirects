using System;

namespace Forte.RedirectMiddleware.Request.HttpRequest
{
    public class HttpModuleHttpRequest : IHttpRequest
    {
        public Uri Url => _httpContext.Request.Url;
        
        private readonly System.Web.HttpContext _httpContext;
        public HttpModuleHttpRequest(System.Web.HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

    }
}