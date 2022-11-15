using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.System
{
    public class SystemRedirectsActions
    {
        private readonly IContentLoader _contentRepository;
        private readonly IRedirectRuleRepository _redirectRuleRepository;

        public SystemRedirectsActions(IContentLoader contentRepository, IRedirectRuleRepository redirectRuleRepository)
        {
            _contentRepository = contentRepository;
            _redirectRuleRepository = redirectRuleRepository;
        }

        public void AddRedirects(PageData pageData, string oldUrl, CultureInfo cultureInfo,
            SystemRedirectReason systemRedirectReason, int priority)
        {
            AddRedirects(pageData, oldUrl, systemRedirectReason, priority);
            HandleChildren(pageData, oldUrl, cultureInfo, systemRedirectReason, priority);
        }

        private void AddRedirects(PageData pageData, string oldUrl, SystemRedirectReason systemRedirectReason, int priority)
        {
            if (!(pageData.Status == VersionStatus.PreviouslyPublished || pageData.Status == VersionStatus.Published))
            {
                return;
            }

            var redirectRule = RedirectRuleModel.NewFromSystem(
                UrlPath.NormalizePath(oldUrl),
                pageData.ContentLink.ID,
                RedirectType.Permanent,
                RedirectRuleType.ExactMatch,
                SystemRedirectsHelper.GetSystemRedirectReason(systemRedirectReason),
                priority);

            try
            {
                _redirectRuleRepository.Add(redirectRule);
            }
            catch (ApplicationException)
            {
            }
        }

        public void DeleteRedirects(ContentReference deletedContent, IEnumerable<ContentReference> deletedDescendants)
        {
            var contentIdsForDeletion = deletedDescendants.Select(x => x.ID).ToList();
            contentIdsForDeletion.Add(deletedContent.ID);
            var redirectsToDelete = _redirectRuleRepository.GetByContent(contentIdsForDeletion);
            foreach (var redirect in redirectsToDelete)
            {
                _redirectRuleRepository.Delete(redirect.Id);
            }
        }

        private void HandleChildren(PageData data, string oldUrl, CultureInfo cultureInfo, SystemRedirectReason systemRedirectReason, int priority)
        {
            var languageSelector = new LanguageSelector(cultureInfo.Name);
            var pageDataCollection = _contentRepository.GetChildren<PageData>(data.PageLink, languageSelector);

            foreach (var pageData in pageDataCollection)
            {
                var oldChildUrl = SystemRedirectsHelper.Combine(oldUrl, pageData.URLSegment);
                AddRedirects(pageData, oldChildUrl, systemRedirectReason, priority);
                HandleChildren(pageData, oldChildUrl, cultureInfo, systemRedirectReason, priority);
            }
        }
    }
}
