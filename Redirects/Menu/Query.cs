using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.RedirectType;

namespace Forte.Redirects.Menu
{
    [ModelBinder(typeof(QueryModelBinder))]
    public class Query
    {
        public string OldPattern { get; set; }
        public string NewPattern { get; set; }
        public RedirectType? RedirectType{ get; set; }
        public RedirectRuleType? RedirectRuleType{ get; set; }
        
        public bool? IsActive{ get; set; }
        public DateTime? CreatedOnFrom{ get; set; }
        public DateTime? CreatedOnTo{ get; set; }
        public string CreatedBy{ get; set; }
        public string Notes{ get; set; }
        public IEnumerable<SortColumn> SortColumns { get; set; }
        public ItemRange Range { get; set; }
    }

    public static class QueryExtension
    {
        public static IEnumerable<RedirectRule> Get(this IQueryable<RedirectRule> redirectRules, Query query = null)
        {
            if (query == null)
                return redirectRules.AsEnumerable();

            if (!string.IsNullOrEmpty(query.OldPattern))
                redirectRules = redirectRules.Where(rr => rr.OldPattern.Contains(query.OldPattern));

            if (!string.IsNullOrEmpty(query.NewPattern))
                redirectRules = redirectRules.Where(rr => rr.NewPattern.Contains(query.NewPattern));

            if (query.RedirectType != null)
                redirectRules = redirectRules.Where(rr => rr.RedirectType == query.RedirectType);

            if (query.RedirectRuleType != null)
                redirectRules = redirectRules.Where(rr => rr.RedirectRuleType == query.RedirectRuleType);
            
            if (query.RedirectRuleType != null)
                redirectRules = redirectRules.Where(rr => rr.IsActive == query.IsActive);
            
            if (query.CreatedOnFrom != null)
                redirectRules = redirectRules.Where(rr => rr.CreatedOn >= query.CreatedOnFrom);
            
            if (query.CreatedOnTo != null)
                redirectRules = redirectRules.Where(rr => rr.CreatedOn <= query.CreatedOnTo);

            if (query.SortColumns != null)
                redirectRules = redirectRules.OrderBy(query.SortColumns);
            
            return redirectRules
                .ApplyRange(query.Range)
                .Items
                .AsEnumerable();
        }
    }
}