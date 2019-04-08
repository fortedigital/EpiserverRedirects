using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace Forte.EpiserverRedirects.UrlRewritePlugin
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

            return urlRewriteModel?.MapToUrlRedirectsDto();
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
                .FirstOrDefault(urlRewriteModel => Regex.IsMatch(oldUrl, urlRewriteModel.OldUrl, RegexOptions.IgnoreCase));
        }

        public static string GetRedirectUrl(int contentId)
        {
            var contentReference = new ContentReference(contentId);
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var virtualPathData = urlResolver.GetVirtualPath(contentReference);

            return virtualPathData?.VirtualPath.NormalizePath();
        }

        public static string GetRedirectUrl(string requestUrl, UrlRedirectsDto urlRewriteModel)
        {
            return urlRewriteModel.Type == UrlRedirectsType.ManualWildcard
                ? Regex.Replace(requestUrl, urlRewriteModel.OldUrl, urlRewriteModel.NewUrl, RegexOptions.IgnoreCase)
                : urlRewriteModel.NewUrl;
        }

        private static void AddRedirectsToDDS(PageData pageData, string oldUrl)
        {
            if (!(pageData.Status == VersionStatus.PreviouslyPublished || pageData.Status == VersionStatus.Published)) return;

            var urlRedirectsDto = new UrlRedirectsDto(
                oldUrl.NormalizePath(), pageData.ContentLink.ID, UrlRedirectsType.System, 1,
                RedirectStatusCode.Permanent);

            var urlRedirectsService = ServiceLocator.Current.GetInstance<IUrlRedirectsService>();

            try
            {
                urlRedirectsService.Put(urlRedirectsDto);
            }
            catch (ApplicationException)
            {
                return;
            }
        }

        public static void DeleteRedirects(ContentReference deletedContent, IEnumerable<ContentReference> deletedDescendants)
        {
            var urlRedirectsService = ServiceLocator.Current.GetInstance<IUrlRedirectsService>();

            var deletedDescendantsIds = deletedDescendants.Select(x => x.ID).ToList();

            var redirectsToDelete = urlRedirectsService.GetAll()
                .Where(x => deletedContent.ID == x.ContentId || deletedDescendantsIds.Contains(x.ContentId))
                .Select(x => x.Id.ExternalId)
                .ToList();

            foreach (var redirect in redirectsToDelete)
            {
                urlRedirectsService.Delete(redirect);
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