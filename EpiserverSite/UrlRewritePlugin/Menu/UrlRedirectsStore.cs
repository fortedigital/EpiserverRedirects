using EPiServer.Data;
using EPiServer.Data.Dynamic;
using EPiServer.Shell.Services.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpiserverSite.UrlRewritePlugin.Menu
{
    [RestStore("UrlRedirectsStore")]
    public class UrlRedirectsStore: RestControllerBase
    {
        private readonly DynamicDataStoreFactory dynamicDataStoreFactory;

        public UrlRedirectsStore(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            this.dynamicDataStoreFactory = dynamicDataStoreFactory;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));
            var urlRewriteStore = store.Items<UrlRewriteModel>();

            return Rest(urlRewriteStore.ToList());
        }

        [HttpPut]
        public ActionResult Put(UrlRewriteModel urlRewriteModel)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));

            store.Save(urlRewriteModel, urlRewriteModel.Id);

            return Rest(urlRewriteModel);
        }

        [HttpPost]
        public ActionResult Post(UrlRewriteModel urlRewriteModel)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));

            var redirectAlredyExist = store.Items<UrlRewriteModel>()
                .FirstOrDefault(x => x.OldUrl == urlRewriteModel.OldUrl);

            if (redirectAlredyExist != null)
            {
                return Rest(false);
            }

            store.Save(urlRewriteModel);

            return Rest(urlRewriteModel);
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));

            store.Delete(Identity.NewIdentity(new Guid(id)));

            return Rest(true);
        }
    }
}