using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Menu
{
    [ModelBinder(typeof(QueryModelBinder))]
    public class Query
    {
        public string OldPattern { get; set; }
        public string NewPattern { get; set; }
        public int? ContentId { get; set; }
        public RedirectType? RedirectType{ get; set; }
        public RedirectRuleType? RedirectRuleType{ get; set; }
        public RedirectOrigin? RedirectOrigin{ get; set; }
        
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
        public static IEnumerable<RedirectRule> GetPageFromQuery(this IQueryable<RedirectRule> redirectRules,
            out int allRedirectsCount, Query query = null)
        {
            if (query == null)
            {
                allRedirectsCount = redirectRules.Count();
                return redirectRules.AsEnumerable();
            }

            if (!string.IsNullOrEmpty(query.OldPattern))
                redirectRules = redirectRules.Where(rr => rr.OldPattern.Contains(query.OldPattern));

            if (!string.IsNullOrEmpty(query.NewPattern))
                redirectRules = redirectRules.Where(rr => rr.NewPattern.Contains(query.NewPattern));
            
            if (query.ContentId !=null)
                redirectRules = redirectRules.Where(rr => rr.ContentId == query.ContentId);

            if (query.RedirectType != null)
                redirectRules = redirectRules.Where(rr => rr.RedirectType == query.RedirectType);

            if (query.RedirectRuleType != null)
                redirectRules = redirectRules.Where(rr => rr.RedirectRuleType == query.RedirectRuleType);
            
            if (query.RedirectOrigin != null)
                redirectRules = redirectRules.Where(rr => rr.RedirectOrigin == query.RedirectOrigin);
            
            if (query.IsActive != null)
                redirectRules = redirectRules.Where(rr => rr.IsActive == query.IsActive);
            
            if (query.CreatedOnFrom != null)
                redirectRules = redirectRules.Where(rr => rr.CreatedOn >= query.CreatedOnFrom);
            
            if (query.CreatedOnTo != null)
                redirectRules = redirectRules.Where(rr => rr.CreatedOn <= query.CreatedOnTo);
            
            if (!string.IsNullOrEmpty(query.CreatedBy))
                redirectRules = redirectRules.Where(rr => rr.CreatedBy.Contains(query.CreatedBy));
            
            if (!string.IsNullOrEmpty(query.Notes))
                redirectRules = redirectRules.Where(rr => rr.Notes.Contains(query.Notes));

            if (query.SortColumns != null)
                redirectRules = redirectRules.OrderBy(query.SortColumns);

            allRedirectsCount = redirectRules.Count();
            
            return redirectRules
                .ApplyRange(query.Range)
                .Items
                .AsEnumerable();
        }
    }
}