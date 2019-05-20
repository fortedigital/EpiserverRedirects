using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Repository;

namespace Forte.Redirects.Import
{
    public class RedirectsImporter
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly IUrlResolver _urlResolver;

        public RedirectsImporter(IRedirectRuleRepository redirectRuleRepository, ISiteDefinitionRepository siteDefinitionRepository, IUrlResolver urlResolver)
        {
            _redirectRuleRepository = redirectRuleRepository;
            _siteDefinitionRepository = siteDefinitionRepository;
            _urlResolver = urlResolver;
        }

        public void ImportRedirects(IEnumerable<RedirectRuleImportRow> redirectsToImport)
        {
            foreach (var redirectDefinition in redirectsToImport)
            {
                var dto = CreateRedirectRule(redirectDefinition);
                _redirectRuleRepository.Add(dto);
            }
        }

        private RedirectRule CreateRedirectRule(RedirectRuleImportRow redirectRow)
        {
            var siteUrls = _siteDefinitionRepository
                .List()
                .Select(x => x.SiteUrl)
                .ToList();

            var contentLink = GetContentLink(siteUrls, redirectRow.NewPattern);

            return contentLink == null
                ? RedirectRule.NewFromImport(redirectRow.OldPattern, redirectRow.NewPattern,
                    Parser.ParseRedirectType(redirectRow.RedirectType),
                    Parser.ParseRedirectRuleType(redirectRow.RedirectRuleType),
                    redirectRow.CreatedBy, redirectRow.Notes)
                : RedirectRule.NewFromImport(redirectRow.OldPattern, contentLink.ID,
                    Parser.ParseRedirectType(redirectRow.RedirectType),
                    Parser.ParseRedirectRuleType(redirectRow.RedirectRuleType), redirectRow.CreatedBy,
                    redirectRow.Notes);
        }

        private ContentReference GetContentLink(IEnumerable<Uri> siteUrls, string redirectRoute)
        {
            foreach (var siteUrl in siteUrls)
            {
                var content = _urlResolver.Route(new UrlBuilder($"{siteUrl}/{redirectRoute}"));
                if (content != null)
                    return content.ContentLink;
            }

            return null;
        }
    }
}