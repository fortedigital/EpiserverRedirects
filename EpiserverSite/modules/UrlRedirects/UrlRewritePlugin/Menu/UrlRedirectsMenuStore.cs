using EPiServer.Data.Dynamic;
using EPiServer.Shell.Services.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EpiserverSite.UrlRewritePlugin.Menu
{
    [RestStore("UrlRedirectsMenuStore")]
    public class UrlRedirectsMenuStore : RestControllerBase
    {
        private readonly DynamicDataStoreFactory dynamicDataStoreFactory;

        public UrlRedirectsMenuStore(DynamicDataStoreFactory dynamicDataStoreFactory)
        {
            this.dynamicDataStoreFactory = dynamicDataStoreFactory;
        }

        [HttpGet]
        public ActionResult Get(string oldUrlSearch, string newUrlSearch, string typeSearch, int? contentIdSearch, IEnumerable<SortColumn> sortColumns, ItemRange range)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));
            var urlRewriteStore = store.Items<UrlRewriteModel>().AsQueryable();

            if (!string.IsNullOrEmpty(oldUrlSearch))
            {
                urlRewriteStore = urlRewriteStore.Where(item => item.OldUrl.Contains(oldUrlSearch));
            }

            if (!string.IsNullOrEmpty(newUrlSearch))
            {
                urlRewriteStore = urlRewriteStore.Where(item => item.NewUrl.Contains(newUrlSearch));
            }

            if (!string.IsNullOrEmpty(typeSearch))
            {
                urlRewriteStore = urlRewriteStore.Where(item => item.Type.Contains(typeSearch));
            }

            if (contentIdSearch != null)
            {
                urlRewriteStore = urlRewriteStore.Where(item => item.ContentId == contentIdSearch.Value);
            }

            var sortedResults = urlRewriteStore
                .OrderBy(sortColumns)
                .Select(item => new
                {
                    Id = item.Id.ExternalId,
                    item.OldUrl,
                    item.NewUrl,
                    item.Type,
                    item.ContentId
                });

            HttpContext.Response.Headers.Add("Content-Range", $"0/{sortedResults.Count()}");
            return Rest(sortedResults.ApplyRange(range).Items.ToList());
        }

        [HttpPut]
        public ActionResult Put(UrlRewriteModel urlRewriteModel, string id)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));
            var guidId = new Guid(id);

            store.Save(urlRewriteModel, guidId);

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

            var guidId = new Guid(id);
            store.Delete(guidId);

            return Rest(true);
        }
    }
}