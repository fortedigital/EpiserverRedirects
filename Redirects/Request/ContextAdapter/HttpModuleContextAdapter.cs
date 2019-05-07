using System.Web;

namespace Forte.RedirectMiddleware.Request.ContextAdapter
{
    public class HttpModuleContextAdapter : ContextAdapter
    {
        private readonly HttpContext _httpContext;
        public HttpModuleContextAdapter(HttpContext httpContext) : base(httpContext.Request.Url)
        {
            _httpContext = httpContext;
        }
        
        public override void Redirect()
        {
            if (_httpContext == null)
                return;
            
            _httpContext.Response.RedirectLocation = Location;
           _httpContext.Response.StatusCode = StatusCode;
        }
    }
}