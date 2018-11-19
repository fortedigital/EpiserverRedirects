using EPiServer;
using EPiServer.Core;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Geta.EPi.Extensions;
using System;
using System.Linq;

namespace EpiserverSite.UrlRewritePlugin
{
    public static class RedirectHelper
    {
        public static void AddRedirects(PageData pageData, string oldUrl, string newUrl)
        {
            AddRedirectsToDDS(oldUrl, newUrl, pageData.ContentLink.ID);
            HandleChildren(pageData, oldUrl, newUrl);
        }

        public static UrlRewriteModel GetRedirectModel(string oldUrl)
        {
            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(UrlRewriteModel));

            return store.Items<UrlRewriteModel>().FirstOrDefault(x => x.OldUrl == oldUrl);
        }

        public static string GetRedirectUrl(int contextId)
        {
            var contentReference = new ContentReference(contextId);
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var virtualPathData = urlResolver.GetVirtualPath(contentReference);

            return virtualPathData?.VirtualPath.NormalizePath();
        }

        private static void AddRedirectsToDDS(string oldUrl, string newUrl, int contextId)
        {
            var urlRewriteModel = new UrlRewriteModel
            {
                OldUrl = oldUrl.NormalizePath(),
                ContextId = contextId
            };
            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(UrlRewriteModel));

            var redirectAlredyExist = store.Items<UrlRewriteModel>()
                .FirstOrDefault(x => x.OldUrl == urlRewriteModel.OldUrl);

            if (redirectAlredyExist != null)
            {
                return;
            }

            //var urlsToUpdate = store.Items<UrlRewriteModel>().Where(x => x.NewUrl == urlRewriteModel.OldUrl).ToList();

            //if (urlsToUpdate.Any())
            //{
            //    foreach (var urlToUpdate in urlsToUpdate)
            //    {
            //        urlToUpdate.NewUrl = urlRewriteModel.NewUrl;
            //        store.Save(urlToUpdate);
            //    }
            //}

            store.Save(urlRewriteModel);
        }

        private static void HandleChildren(PageData data, string oldUrl, string newUrl)
        {
            var pageDataCollection = data.GetChildren();

            foreach (var pageData in pageDataCollection)
            {
                var oldChildUrl = Combine(oldUrl, pageData.URLSegment);
                var newChildUrl = Combine(newUrl, pageData.URLSegment);

                AddRedirectsToDDS(oldChildUrl, newChildUrl, pageData.ContentLink.ID);
                HandleChildren(pageData, oldChildUrl, newChildUrl);
            }
        }

        private static string Combine(string str1, string str2)
        {
            str1 = str1.TrimEnd('/');
            str2 = str2.TrimStart('/');
            return $"{str1}/{str2}";
        }
    }
}