using EPiServer.Core;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Geta.EPi.Extensions;
using System.Linq;
using System.Text.RegularExpressions;

namespace EpiserverSite.UrlRewritePlugin
{
    public static class RedirectHelper
    {
        public static void AddRedirects(PageData pageData, string oldUrl)
        {
            AddRedirectsToDDS(oldUrl, pageData.ContentLink.ID);
            HandleChildren(pageData, oldUrl);
        }

        public static UrlRewriteModel GetRedirectModel(string oldUrl)
        {
            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(UrlRewriteModel));

            var urlRewriteModel = store.Items<UrlRewriteModel>().GetRedirectModel(oldUrl);

            if (urlRewriteModel == null)
            {
                urlRewriteModel = store.Items<UrlRewriteModel>().GetManualWildcardTypeRedirectModel(oldUrl);
            }

            return urlRewriteModel;
        }

        private static UrlRewriteModel GetRedirectModel(this IOrderedQueryable<UrlRewriteModel> urlRewriteStore, string oldUrl)
        {
            return urlRewriteStore.FirstOrDefault(x => x.OldUrl == oldUrl);
        }

        private static UrlRewriteModel GetManualWildcardTypeRedirectModel(this IOrderedQueryable<UrlRewriteModel> urlRewriteStore, string oldUrl)
        {
            return urlRewriteStore.Where(x => x.Type == "manual-wildcard")
                .OrderBy(urlRewriteModel => urlRewriteModel.Priority)
                .AsEnumerable()
                .FirstOrDefault(urlRewriteModel => Regex.IsMatch(oldUrl, urlRewriteModel.OldUrl));
        }

        public static string GetRedirectUrl(int contentId)
        {
            var contentReference = new ContentReference(contentId);
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var virtualPathData = urlResolver.GetVirtualPath(contentReference);

            return virtualPathData?.VirtualPath.NormalizePath();
        }

        public static string GetRedirectUrl(string oldUrl, UrlRewriteModel urlRewriteModel)
        {
            var regexMatch = Regex.Match(oldUrl, urlRewriteModel.OldUrl);
            string redirectUrl = urlRewriteModel.NewUrl;

            for (int i = 1; i < regexMatch.Groups.Count; i++)
            {
                redirectUrl = redirectUrl.Replace("{$" + i + "}", regexMatch.Groups[i].ToString());
            }

            return redirectUrl;
        }

        private static void AddRedirectsToDDS(string oldUrl, int contentId)
        {
            var urlRewriteModel = new UrlRewriteModel
            {
                OldUrl = oldUrl.NormalizePath(),
                ContentId = contentId,
                Type = "system",
                Priority = 1
            };

            AddRedirectsToDDS(urlRewriteModel);
        }

        private static void AddRedirectsToDDS(UrlRewriteModel urlRewriteModel)
        {
            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(UrlRewriteModel));

            var redirectAlredyExist = store.Items<UrlRewriteModel>()
                .FirstOrDefault(x => x.OldUrl == urlRewriteModel.OldUrl);

            if (redirectAlredyExist != null)
            {
                return;
            }

            store.Save(urlRewriteModel);
        }

        private static void HandleChildren(PageData data, string oldUrl)
        {
            var pageDataCollection = data.GetChildren();

            foreach (var pageData in pageDataCollection)
            {
                var oldChildUrl = Combine(oldUrl, pageData.URLSegment);
                AddRedirectsToDDS(oldChildUrl, pageData.ContentLink.ID);
                HandleChildren(pageData, oldChildUrl);
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