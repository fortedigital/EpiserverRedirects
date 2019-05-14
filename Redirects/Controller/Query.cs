using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.Services.Rest;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;

namespace Forte.RedirectMiddleware.Controller
{
    public class Query
    {
        public string OldPath { get; set; }
        public string OldPattern { get; set; }
        public string NewPattern { get; set; }
        public RedirectType? RedirectType{ get; set; }
        public RedirectRuleType? RedirectRuleType{ get; set; }
        public IEnumerable<SortColumn> SortColumns { get; set; }
        public ItemRange Range { get; set; }
    }

    public static class QueryExtension
    {
        public static IEnumerable<RedirectRule> Get(this IQueryable<RedirectRule> redirectRules, Query query)
        {
            if (query == null)
                return redirectRules.AsEnumerable();
            
            if (!string.IsNullOrEmpty(query.OldPath))
                redirectRules = redirectRules.Where(rr => rr.OldPath.ToString().Contains(query.OldPath));

            if (!string.IsNullOrEmpty(query.OldPattern))
                redirectRules = redirectRules.Where(rr => rr.OldPattern.Contains(query.OldPattern));

            if (!string.IsNullOrEmpty(query.NewPattern))
                redirectRules = redirectRules.Where(rr => rr.NewPattern.Contains(query.NewPattern));

            if (query.RedirectType != null)
                redirectRules = redirectRules.Where(rr => rr.RedirectType == query.RedirectType);

            if (query.RedirectRuleType != null)
                redirectRules = redirectRules.Where(rr => rr.RedirectRuleType == query.RedirectRuleType);

            return redirectRules
                .OrderBy(query.SortColumns)
                .ApplyRange(query.Range)
                .Items
                .AsEnumerable();
        }
    }
}