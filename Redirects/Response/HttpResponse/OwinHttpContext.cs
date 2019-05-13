using Microsoft.Owin;

namespace Forte.RedirectMiddleware.Response.HttpResponse
{
    public class OwinHttpResponse : IHttpResponse
    {
        private readonly IOwinContext _owinContext;
        public OwinHttpResponse(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public void Redirect(string location, int statusCode)
        {
            _owinContext.Response.Headers.Set("Location", location);
            _owinContext.Response.StatusCode = statusCode;
        }
    }
}