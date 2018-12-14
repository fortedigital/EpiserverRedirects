using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;

namespace Forte.EpiserverRedirects.UrlRewritePlugin.Menu
{
    [RestStore("UrlRedirectsMenuStore")]
    public class UrlRedirectsMenuStore : RestControllerBase
    {
        private readonly IUrlRedirectsService urlRedirectsService;

        public UrlRedirectsMenuStore(IUrlRedirectsService urlRedirectsService)
        {
            this.urlRedirectsService = urlRedirectsService;
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
            var urlRewriteStore = urlRedirectsService.GetAll();

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
                    .Where(urlRewriteModel => urlRewriteModel.Type == UrlRedirectsType.ManualWildcard.ToString())
                    .AsEnumerable()
                    .Where(urlRewriteModel => Regex.IsMatch(simulatedOldUrl, urlRewriteModel.OldUrl))
                    .AsQueryable();
            }

            var results = urlRewriteStore
                .OrderBy(sortColumns)
                .ApplyRange(range)
                .Items.AsEnumerable()
                .Select(item => item.MapToUrlRedirectsDtoModel());

            HttpContext.Response.Headers.Add("Content-Range", $"0/{urlRedirectsService.GetAll().Count()}");
            return Rest(results);
        }

        [HttpPut]
        public ActionResult Put(UrlRedirectsDto urlRedirectsDto)
        {
            try
            {
                var urlRewriteModel = urlRedirectsService.Put(urlRedirectsDto);

                return Rest(urlRewriteModel);
            }
            catch (ApplicationException)
            {
                return new RestStatusCodeResult(HttpStatusCode.Conflict);
            }
        }

        [HttpPost]
        public ActionResult Post(UrlRedirectsDto urlRedirectsDto)
        {
            try
            {
                var urlRewriteModel = urlRedirectsService.Post(urlRedirectsDto);

                return Rest(urlRewriteModel);
            }
            catch (ApplicationException)
            {
                return new RestStatusCodeResult(HttpStatusCode.Conflict);
            }
        }

        [HttpDelete]
        public ActionResult Delete(Guid id)
        {
            urlRedirectsService.Delete(id);

            return Rest(HttpStatusCode.OK);
        }
    }
}