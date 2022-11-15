using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System.Collections.Generic;
using System.Linq;


namespace Forte.EpiserverRedirects.DynamicDataStore
{
    public static class QueryExtension
    {
        public static IEnumerable<DdsRedirectRule> ApplyQuery(this IQueryable<DdsRedirectRule> redirectRules,
            out int allRedirectsCount, RedirectRuleQuery query = null)
        {
            if (query == null)
            {
                allRedirectsCount = redirectRules.Count();
                return redirectRules.AsEnumerable();
            }

            if (!string.IsNullOrEmpty(query.OldPattern))
            {
                redirectRules = redirectRules.Where(rr => rr.OldPattern.Contains(query.OldPattern));
            }

            if (!string.IsNullOrEmpty(query.NewPattern))
            {
                redirectRules = redirectRules.Where(rr => rr.NewPattern.Contains(query.NewPattern));
            }

            if (query.ContentId != null)
            {
                redirectRules = redirectRules.Where(rr => rr.ContentId == query.ContentId);
            }

            if (query.RedirectType != null)
            {
                redirectRules = redirectRules.Where(rr => rr.RedirectType == query.RedirectType);
            }

            if (query.RedirectRuleType != null)
            {
                redirectRules = redirectRules.Where(rr => rr.RedirectRuleType == query.RedirectRuleType);
            }

            if (query.RedirectOrigin != null)
            {
                redirectRules = redirectRules.Where(rr => rr.RedirectOrigin == query.RedirectOrigin);
            }

            if (query.IsActive != null)
            {
                redirectRules = redirectRules.Where(rr => rr.IsActive == query.IsActive);
            }

            if (query.CreatedOnFrom != null)
            {
                redirectRules = redirectRules.Where(rr => rr.CreatedOn >= query.CreatedOnFrom);
            }

            if (query.CreatedOnTo != null)
            {
                redirectRules = redirectRules.Where(rr => rr.CreatedOn <= query.CreatedOnTo);
            }

            if (!string.IsNullOrEmpty(query.CreatedBy))
            {
                redirectRules = redirectRules.Where(rr => rr.CreatedBy.Contains(query.CreatedBy));
            }

            if (!string.IsNullOrEmpty(query.Notes))
            {
                redirectRules = redirectRules.Where(rr => rr.Notes.Contains(query.Notes));
            }

            if (query.Priority != null)
            {
                redirectRules = redirectRules.Where(rr => rr.Priority <= query.Priority);
            }

            if (query.SortColumns != null)
            {
                redirectRules = redirectRules.OrderBy(query.SortColumns);
            }

            allRedirectsCount = redirectRules.Count();

            return redirectRules
                .ApplyRange(query.Range)
                .Items
                .AsEnumerable();
        }
    }
}
