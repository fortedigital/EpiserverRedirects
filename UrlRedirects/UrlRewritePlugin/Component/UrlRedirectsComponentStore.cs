using System.Linq;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;

namespace Forte.UrlRedirects.UrlRewritePlugin.Component
{
    [RestStore("UrlRedirectsComponentStore")]
    public class UrlRedirectsComponentStore : RestControllerBase
    {
        private readonly IUrlRedirectsService urlRedirectsService;

        public UrlRedirectsComponentStore(IUrlRedirectsService urlRedirectsService)
        {
            this.urlRedirectsService = urlRedirectsService;
        }

        [HttpGet]
        public ActionResult Get(int contentId, string filter)
        {
            var result = urlRedirectsService.GetAll()
                .Where(item => item.OldUrl.Contains(filter))
                .Where(item => item.ContentId == contentId);

            return Rest(result.ToList());
        }
    }
}