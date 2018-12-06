using EPiServer.Data.Dynamic;
using EPiServer.Shell.Services.Rest;
using System;
using System.Linq;
using System.Web.Mvc;

namespace UrlRedirects.UrlRewritePlugin.Component
{
    [RestStore("UrlRedirectsComponentStore")]
    public class UrlRedirectsComponentStore : RestControllerBase
    {
        private readonly DynamicDataStoreFactory dynamicDataStoreFactory;

        public UrlRedirectsComponentStore(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            this.dynamicDataStoreFactory = dynamicDataStoreFactory;
        }

        [HttpGet]
        public ActionResult Get(int contentId, string filter)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));

            var urlRewriteStore = store.Items<UrlRewriteModel>();
            var result = urlRewriteStore
                .Where(item => item.OldUrl.Contains(filter))
                .Where(item => item.ContentId == contentId);

            return Rest(result.ToList());
        }
    }
}