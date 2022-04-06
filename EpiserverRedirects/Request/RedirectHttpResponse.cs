using Forte.EpiserverRedirects.Model.RedirectRule;
using Microsoft.AspNetCore.Http;

namespace Forte.EpiserverRedirects.Request
{
    public class RedirectHttpResponse : IRedirectHttpResponse
    {
        private readonly HttpResponse _response;

        public RedirectHttpResponse(HttpResponse response)
        {
            _response = response;
        }

        public void Redirect(string location, RedirectType redirectType)
        {
            _response.Redirect(location, redirectType == RedirectType.Permanent);
        }
    }
}
