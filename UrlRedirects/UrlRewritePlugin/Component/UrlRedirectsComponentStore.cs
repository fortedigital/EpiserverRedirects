using EPiServer.Data.Dynamic;
using EPiServer.Shell.Services.Rest;
using System;
using System.Linq;
using System.Web.Mvc;
using Test.modules.UrlRedirects.UrlRewritePlugin;

namespace UrlRedirects.UrlRewritePlugin.Component
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