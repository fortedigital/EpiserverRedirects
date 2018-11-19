using EPiServer.Data.Dynamic;
using EPiServer.Shell.Services.Rest;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EpiserverSite.UrlRewritePlugin
{
    [RestStore("urlRewriteStore")]
    public class UrlRewriteStore : RestControllerBase
    {
        private readonly DynamicDataStoreFactory dynamicDataStoreFactory;

        public UrlRewriteStore(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            this.dynamicDataStoreFactory = dynamicDataStoreFactory;
        }

        [HttpGet]
        public ActionResult Get(Guid Id, string filter)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));
            var urlRewriteStore = store.Items<UrlRewriteModel>();
            var result = urlRewriteStore
                .Where(item => item.OldUrl.Contains(filter))
                .Where(item => item.ContentGuid == Id);

            return Rest(result.ToList());
        }

    }
}