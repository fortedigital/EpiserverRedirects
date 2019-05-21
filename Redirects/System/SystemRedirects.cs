using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Forte.Redirects.Menu;
using Forte.Redirects.Model;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Repository;

namespace Forte.Redirects.System
{
    public static class SystemRedirects
    {
        public static void AddRedirects(PageData pageData, string oldUrl, CultureInfo cultureInfo)
        {
            AddRedirects(pageData, oldUrl);
            HandleChildren(pageData, oldUrl, cultureInfo);
        }
        
        private static void AddRedirects(PageData pageData, string oldUrl)
        {
            if (!(pageData.Status == VersionStatus.PreviouslyPublished || pageData.Status == VersionStatus.Published))
                return;

            var redirectRule = RedirectRule.NewFromSystem(
                UrlPath.NormalizePath(oldUrl),
                pageData.ContentLink.ID,
                RedirectType.Permanent,
                RedirectRuleType.ExactMatch,
                "");
            
            var urlRedirectsService = ServiceLocator.Current.GetInstance<IRedirectRuleRepository>();

            try
            {
                urlRedirectsService.Add(redirectRule);
            }
            catch (ApplicationException)
            {
                return;
            }
        }

        public static void DeleteRedirects(ContentReference deletedContent, IEnumerable<ContentReference> deletedDescendants)
        {
            var urlRedirectsService = ServiceLocator.Current.GetInstance<IRedirectRuleRepository>();

            var deletedDescendantsIds = deletedDescendants.Select(x => x.ID).ToList();

            var redirectsToDelete = urlRedirectsService.Get()
                .Where(x => deletedContent.ID == x.ContentId || deletedDescendantsIds.Contains(x.ContentId.Value))
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
                AddRedirects(pageData, oldChildUrl);
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