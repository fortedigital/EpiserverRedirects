using Forte.Redirects.Request;

namespace Forte.Redirects.AspNet
{
    public class HttpModuleHttpResponse : IHttpResponse
    {
        private readonly global::System.Web.HttpResponse _httpResponse;
        public HttpModuleHttpResponse(global::System.Web.HttpResponse httpResponse)
        {
            _httpResponse = httpResponse;
        }
        
        public void Redirect(string location, int statusCode)
        { 
            _httpResponse.RedirectLocation = location; 
            _httpResponse.StatusCode = statusCode;
        }
    }
}