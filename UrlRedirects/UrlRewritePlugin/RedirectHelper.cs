using EPiServer;
using EPiServer.Core;
using EPiServer.Data.Dynamic;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Test.modules.UrlRedirects.UrlRewritePlugin;

namespace UrlRedirects.UrlRewritePlugin
{
    public static class RedirectHelper
    {
        public static void AddRedirects(PageData pageData, string oldUrl, CultureInfo cultureInfo)
        {
            AddRedirectsToDDS(pageData, oldUrl);
            HandleChildren(pageData, oldUrl, cultureInfo);
        }

        public static UrlRedirectsDto GetRedirectModel(string oldUrl)
        {
            var urlRedirectsService = ServiceLocator.Current.GetInstance<IUrlRedirectsService>();
            var urlRewriteModels = urlRedirectsService.GetAll();
            var urlRewriteModel = urlRewriteModels.GetRedirectModel(oldUrl) ?? urlRewriteModels.GetManualWildcardTypeRedirectModel(oldUrl);

            return urlRewriteModel?.MapToUrlRedirectsDtoModel();
        }

        private static UrlRewriteModel GetRedirectModel(this IQueryable<UrlRewriteModel> urlRewriteStore, string oldUrl)
        {
            return urlRewriteStore
                .FirstOrDefault(x => x.OldUrl == oldUrl);
        }

        private static UrlRewriteModel GetManualWildcardTypeRedirectModel(this IQueryable<UrlRewriteModel> urlRewriteStore, string oldUrl)
        {
            return urlRewriteStore.Where(x => x.Type == UrlRedirectsType.ManualWildcard.ToString())
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

        public static string GetRedirectUrl(string oldUrl, UrlRedirectsDto urlRewriteModel)
        {
            return Regex.Replace(oldUrl, urlRewriteModel.OldUrl, urlRewriteModel.NewUrl);
        }

        private static void AddRedirectsToDDS(PageData pageData, string oldUrl)
        {
            if (!(pageData.Status == VersionStatus.PreviouslyPublished || pageData.Status == VersionStatus.Published)) return;
            
            var urlRedirectsDto = new UrlRedirectsDto
            {
                OldUrl = oldUrl.NormalizePath(),
                ContentId = pageData.ContentLink.ID,
                Type = UrlRedirectsType.System,
                Priority = 1,
                RedirectStatusCode = RedirectStatusCode.Permanent
            };

            var urlRedirectsService = ServiceLocator.Current.GetInstance<IUrlRedirectsService>();

            try
            {
                urlRedirectsService.Post(urlRedirectsDto);
            }
            catch (ApplicationException)
            {
                return;
            }
        }

        private static void HandleChildren(PageData data, string oldUrl, CultureInfo cultureInfo)
        {
            var languageSelector = new LanguageSelector(cultureInfo.Name);
            var pageDataCollection = DataFactory.Instance.GetChildren(data.PageLink, languageSelector);

            foreach (var pageData in pageDataCollection)
            {
                var oldChildUrl = Combine(oldUrl, pageData.URLSegment);
                AddRedirectsToDDS(pageData, oldChildUrl);
                HandleChildren(pageData, oldChildUrl, cultureInfo);
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