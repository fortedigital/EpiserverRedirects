using Forte.RedirectMiddleware.Request;

namespace HttpModule
{
    public class HttpModuleHttpResponse : IHttpResponse
    {
        private readonly System.Web.HttpResponse _httpResponse;
        public HttpModuleHttpResponse(System.Web.HttpResponse httpResponse)
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