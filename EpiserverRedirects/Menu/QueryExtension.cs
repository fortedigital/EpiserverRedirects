using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;


namespace Forte.EpiserverRedirects.Menu
{
    public static class QueryExtension
    {
        public static IEnumerable<IRedirectRule> GetPageFromQuery(this IQueryable<IRedirectRule> rules,
            out int allRedirectsCount, Query query = null)
        {
            if (query == null)
            {
                allRedirectsCount = rules.Count();
                return rules.AsEnumerable();
            }

            if (!string.IsNullOrEmpty(query.OldPattern))
            {
                rules = rules.Where(rr => rr.OldPattern.Contains(query.OldPattern));
            }

            if (!string.IsNullOrEmpty(query.NewPattern))
            {
                rules = rules.Where(rr => rr.NewPattern.Contains(query.NewPattern));
            }

            if (query.ContentId != null)
            {
                rules = rules.Where(rr => rr.ContentId == query.ContentId);
            }

            if (query.RedirectType != null)
            {
                rules = rules.Where(rr => rr.RedirectType == query.RedirectType);
            }

            if (query.RedirectRuleType != null)
            {
                rules = rules.Where(rr => rr.RedirectRuleType == query.RedirectRuleType);
            }

            if (query.RedirectOrigin != null)
            {
                rules = rules.Where(rr => rr.RedirectOrigin == query.RedirectOrigin);
            }

            if (query.IsActive != null)
            {
                rules = rules.Where(rr => rr.IsActive == query.IsActive);
            }

            if (query.CreatedOnFrom != null)
            {
                rules = rules.Where(rr => rr.CreatedOn >= query.CreatedOnFrom);
            }

            if (query.CreatedOnTo != null)
            {
                rules = rules.Where(rr => rr.CreatedOn <= query.CreatedOnTo);
            }

            if (!string.IsNullOrEmpty(query.CreatedBy))
            {
                rules = rules.Where(rr => rr.CreatedBy.Contains(query.CreatedBy));
            }

            if (!string.IsNullOrEmpty(query.Notes))
            {
                rules = rules.Where(rr => rr.Notes.Contains(query.Notes));
            }

            if (query.Priority != null)
            {
                rules = rules.Where(rr => rr.Priority <= query.Priority);
            }

            IOrderedQueryable<IRedirectRule> orderedRules = null;
            foreach (var column in query.SortColumns)
            {
                var sprtExpr = column.SortDescending
                    ? column.ColumnName + " desc"
                    : column.ColumnName;
                orderedRules = orderedRules == null
                    ? rules.OrderBy(sprtExpr)
                    : orderedRules.ThenBy(sprtExpr);
                rules = orderedRules;
            }

            allRedirectsCount = rules.Count();
            return rules
                .ApplyRange(query.Range)
                .Items
                .AsEnumerable();
        }
    }
}
