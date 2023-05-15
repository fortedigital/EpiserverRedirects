using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using EPiServer.Web;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Import
{
    public class RedirectsImporter
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly RedirectsOptions _options;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private IEnumerable<SiteDefinition> _allHosts;

        public RedirectsImporter(IRedirectRuleRepository redirectRuleRepository, RedirectsOptions options, ISiteDefinitionRepository siteDefinitionRepository)
        {
            _redirectRuleRepository = redirectRuleRepository;
            _options = options;
            _siteDefinitionRepository = siteDefinitionRepository;
        }
        public IEnumerable<SiteDefinition> AllHosts
        {
            get { return _allHosts ??= _siteDefinitionRepository.List(); }
        }

        public void ImportRedirects(IEnumerable<RedirectRuleImportRow> redirectsToImport)
        {
            _redirectRuleRepository.AddRange(redirectsToImport.Select(CreateRedirectRule));
        }

        private IRedirectRule CreateRedirectRule(RedirectRuleImportRow redirectRow)
        {
            var matchToContent = redirectRow.ContentId.HasValue;
            
            return matchToContent == false
                ? RedirectRuleModel.NewFromImport(redirectRow.OldPattern, redirectRow.NewPattern,
                    Parser.ParseRedirectType(redirectRow.RedirectType),
                    Parser.ParseRedirectRuleType(redirectRow.RedirectRuleType),
                    Parser.ParseBoolean(redirectRow.IsActive),
                    redirectRow.Notes,
                    redirectRow.Priority ?? _options.DefaultRedirectRulePriority, DetermineHostId(redirectRow.Host))
                : RedirectRuleModel.NewFromImport(redirectRow.OldPattern, redirectRow.ContentId.Value,
                    Parser.ParseRedirectType(redirectRow.RedirectType),
                    Parser.ParseRedirectRuleType(redirectRow.RedirectRuleType),
                    Parser.ParseBoolean(redirectRow.IsActive),
                    redirectRow.Notes,
                    redirectRow.Priority ?? _options.DefaultRedirectRulePriority, DetermineHostId(redirectRow.Host));
        }

        private Guid? DetermineHostId(string hostIdOrHostName)
        {
            if (string.IsNullOrEmpty(hostIdOrHostName))
            {
                return null;
            }
            if (Guid.TryParse(hostIdOrHostName, out var guid))
            {
                if (AllHosts.Where(s => s.Id == guid).IsNullOrEmpty())
                {
                    return null;
                }

                return guid;
            }
            return AllHosts.FirstOrDefault(s => s.Name.Equals(hostIdOrHostName, StringComparison.InvariantCultureIgnoreCase))?.Id;
        }
    }
}
