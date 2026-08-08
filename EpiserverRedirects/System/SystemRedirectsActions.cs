using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
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

        public void AddRedirects(IContent content, string oldUrl, CultureInfo cultureInfo,
            SystemRedirectReason systemRedirectReason, int priority)
        {
            AddRedirects(content, oldUrl, systemRedirectReason, priority);
            HandleChildren(content, oldUrl, cultureInfo, systemRedirectReason, priority);
        }

        private void AddRedirects(IContent content, string oldUrl, SystemRedirectReason systemRedirectReason, int priority)
        {
            var status = (content as IVersionable)?.Status;
            if (status != VersionStatus.PreviouslyPublished && status != VersionStatus.Published)
            {
                return;
            }

            var redirectRule = RedirectRuleModel.NewFromSystem(
                UrlPath.NormalizePath(oldUrl),
                content.ContentLink.ID,
                content.ContentLink.ProviderName,
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
            // We cast this list to nullable as ContentId in the redirect rule is nullable as well SQL query builder had
            // issues with converting expression containing <nullable>.HasValue
            var deletedDescendantsIds = deletedDescendants.Select(x => (int?)x.ID).ToList();

            // Episerver DDS does not handle query with Contains and empty collection
            var redirectsToDelete = deletedDescendantsIds.Any()
                ? _redirectRuleRepository
                    .GetAll()
                    .Where(x => deletedContent.ID == x.ContentId || deletedDescendantsIds.Contains(x.ContentId))
                    .ToList()
                : _redirectRuleRepository
                    .GetAll()
                    .Where(x => deletedContent.ID == x.ContentId)
                    .ToList();

            foreach (var redirect in redirectsToDelete)
            {
                _redirectRuleRepository.Delete(redirect.RuleId);
            }
        }

        private void HandleChildren(IContent content, string oldUrl, CultureInfo cultureInfo, SystemRedirectReason systemRedirectReason, int priority)
        {
            var languageSelector = new LanguageSelector(cultureInfo.Name);
            var children = _contentRepository.GetChildren<IContent>(content.ContentLink, languageSelector);

            foreach (var child in children)
            {
                var routeSegment = (child as IRoutable)?.RouteSegment;
                if (routeSegment == null)
                {
                    continue;
                }

                var oldChildUrl = SystemRedirectsHelper.Combine(oldUrl, routeSegment);
                AddRedirects(child, oldChildUrl, systemRedirectReason, priority);
                HandleChildren(child, oldChildUrl, cultureInfo, systemRedirectReason, priority);
            }
        }
    }
}
