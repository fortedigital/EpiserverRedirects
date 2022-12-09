using System.Collections.Generic;
using System.Linq;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Import
{
    public class RedirectsImporter
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly RedirectsOptions _options;

        public RedirectsImporter(IRedirectRuleRepository redirectRuleRepository, RedirectsOptions options)
        {
            _redirectRuleRepository = redirectRuleRepository;
            _options = options;
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
                    redirectRow.Priority ?? _options.DefaultRedirectRulePriority)
                : RedirectRuleModel.NewFromImport(redirectRow.OldPattern, redirectRow.ContentId.Value,
                    Parser.ParseRedirectType(redirectRow.RedirectType),
                    Parser.ParseRedirectRuleType(redirectRow.RedirectRuleType),
                    Parser.ParseBoolean(redirectRow.IsActive),
                    redirectRow.Notes,
                    redirectRow.Priority ?? _options.DefaultRedirectRulePriority);
        }
    }
}
