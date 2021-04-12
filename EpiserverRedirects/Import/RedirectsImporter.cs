using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Import
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
            var matchToContent = redirectRow.ContentId.HasValue;
            
            return matchToContent == false
                ? RedirectRule.NewFromImport(redirectRow.OldPattern, redirectRow.NewPattern,
                    Parser.ParseRedirectType(redirectRow.RedirectType),
                    Parser.ParseRedirectRuleType(redirectRow.RedirectRuleType),
                    Parser.ParseBoolean(redirectRow.IsActive),
                    redirectRow.Notes,
                    redirectRow.Priority)
                : RedirectRule.NewFromImport(redirectRow.OldPattern, redirectRow.ContentId.Value,
                    Parser.ParseRedirectType(redirectRow.RedirectType),
                    Parser.ParseRedirectRuleType(redirectRow.RedirectRuleType),
                    Parser.ParseBoolean(redirectRow.IsActive),
                    redirectRow.Notes,
                    redirectRow.Priority);
        }
    }
}
