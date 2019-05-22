using Forte.EpiserverRedirects.Request;
using Microsoft.Owin;

namespace Forte.EpiserverRedirects.AspNet
{
    public class OwinHttpResponse : IHttpResponse
    {
        private readonly IOwinResponse _owinResponse;
        public OwinHttpResponse(IOwinResponse owinResponse)
        {
            _owinResponse = owinResponse;
        }

        public void Redirect(string location, int statusCode)
        {
            _owinResponse.Headers.Set("Location", location);
            _owinResponse.StatusCode = statusCode;
        }
    }
}