using EPiServer.Data.Dynamic;
using EPiServer.Shell.Services.Rest;
using EpiserverSite.modules.UrlRedirects.UrlRewritePlugin.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
        public ActionResult Get(
            string oldUrlSearch,
            string newUrlSearch,
            string typeSearch, 
            int? prioritySearch, 
            string simulatedOldUrl, 
            int? redirectStatusCodeSearch,
            IEnumerable<SortColumn> sortColumns,
            ItemRange range)
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
                urlRewriteStore = urlRewriteStore.Where(item => item.Type == typeSearch);
            }

            if (prioritySearch != null)
            {
                urlRewriteStore = urlRewriteStore.Where(item => item.Priority == prioritySearch.Value);
            }

            if (redirectStatusCodeSearch != null)
            {
                urlRewriteStore = urlRewriteStore.Where(item => item.RedirectStatusCode == redirectStatusCodeSearch.Value);
            }

            if (!string.IsNullOrEmpty(simulatedOldUrl))
            {
                urlRewriteStore = urlRewriteStore
                    .Where(urlRewriteModel => urlRewriteModel.Type == "manual-wildcard")
                    .AsEnumerable()
                    .Where(urlRewriteModel => Regex.IsMatch(simulatedOldUrl, urlRewriteModel.OldUrl))
                    .AsQueryable();
            }

            var results = urlRewriteStore
                .OrderBy(sortColumns)
                .ApplyRange(range)
                .Items.AsEnumerable()
                .Select(item => item.MapToUrlRedirectsMenuViewModel());

            HttpContext.Response.Headers.Add("Content-Range", $"0/{store.Items<UrlRewriteModel>().Count()}");
            return Rest(results);
        }

        [HttpPut]
        public ActionResult Put(UrlRedirectsMenuViewModel urlRedirectsMenuViewModel)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));
            var urlRewriteModel = urlRedirectsMenuViewModel.MapToUrlRewriteModel();

            var redirectAlredyExist = store.Items<UrlRewriteModel>()
                .FirstOrDefault(x => x.OldUrl == urlRewriteModel.OldUrl && x.Id.ExternalId != urlRedirectsMenuViewModel.Id);

            if (redirectAlredyExist != null)
            {
                return new RestStatusCodeResult(HttpStatusCode.Conflict);
            }

            store.Save(urlRewriteModel, urlRedirectsMenuViewModel.Id);

            return Rest(urlRewriteModel);
        }

        [HttpPost]
        public ActionResult Post(UrlRedirectsMenuViewModel urlRedirectsMenuViewModel)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));
            var urlRewriteModel = urlRedirectsMenuViewModel.MapToUrlRewriteModel();

            var redirectAlredyExist = store.Items<UrlRewriteModel>()
                .FirstOrDefault(x => x.OldUrl == urlRewriteModel.OldUrl);

            if (redirectAlredyExist != null)
            {
                return new RestStatusCodeResult(HttpStatusCode.Conflict);
            }

            store.Save(urlRewriteModel);

            return Rest(urlRewriteModel);
        }

        [HttpDelete]
        public ActionResult Delete(Guid id)
        {
            var store = dynamicDataStoreFactory.CreateStore(typeof(UrlRewriteModel));

            store.Delete(id);

            return Rest(true);
        }
    }
}