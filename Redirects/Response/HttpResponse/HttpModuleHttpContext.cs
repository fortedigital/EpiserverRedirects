namespace Forte.RedirectMiddleware.Response.HttpResponse
{
    public class HttpModuleHttpResponse : IHttpResponse
    {
        private readonly System.Web.HttpContext _httpContext;
        public HttpModuleHttpResponse(System.Web.HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        
        public void Redirect(string location, int statusCode)
        { 
            _httpContext.Response.RedirectLocation = location; 
            _httpContext.Response.StatusCode = statusCode;
        }
    }
}