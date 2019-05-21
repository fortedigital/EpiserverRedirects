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
    public static class SystemRedirectsActions
    {
        public static void AddRedirects(PageData pageData, string oldUrl, CultureInfo cultureInfo,
            SystemRedirectReason systemRedirectReason)
        {
            AddRedirects(pageData, oldUrl, systemRedirectReason);
            HandleChildren(pageData, oldUrl, cultureInfo, systemRedirectReason);
        }

        private static void AddRedirects(PageData pageData, string oldUrl, SystemRedirectReason systemRedirectReason)
        {
            if (!(pageData.Status == VersionStatus.PreviouslyPublished || pageData.Status == VersionStatus.Published))
                return;

            var redirectRule = RedirectRule.NewFromSystem(
                UrlPath.NormalizePath(oldUrl),
                pageData.ContentLink.ID,
                RedirectType.Permanent,
                RedirectRuleType.ExactMatch,
                SystemRedirectsHelper.GetSystemRedirectReason(systemRedirectReason));

            var urlRedirectsService = ServiceLocator.Current.GetInstance<IRedirectRuleRepository>();

            try
            {
                urlRedirectsService.Add(redirectRule);
            }
            catch (ApplicationException) { }
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

        private static void HandleChildren(PageData data, string oldUrl, CultureInfo cultureInfo,
            SystemRedirectReason systemRedirectReason)
        {
            var languageSelector = new LanguageSelector(cultureInfo.Name);
            var pageDataCollection = DataFactory.Instance.GetChildren(data.PageLink, languageSelector);

            foreach (var pageData in pageDataCollection)
            {
                var oldChildUrl = SystemRedirectsHelper.Combine(oldUrl, pageData.URLSegment);
                AddRedirects(pageData, oldChildUrl, systemRedirectReason);
                HandleChildren(pageData, oldChildUrl, cultureInfo, systemRedirectReason);
            }
        }


    }
}