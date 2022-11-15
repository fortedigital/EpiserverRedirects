using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.DynamicDataStore;
using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Forte.EpiserverRedirects.EntityFramework.Repository
{
    public static class QueryExtension
    {
        public static IEnumerable<RedirectRuleEntity> ApplyQuery(this IQueryable<RedirectRuleEntity> redirectRules,
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
                foreach (var column in query.SortColumns)
                {
                    if (string.Equals(nameof(DdsRedirectRule.ContentId), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.ContentId) :
                            redirectRules.OrderBy(o => o.ContentId);
                        continue;
                    }

                    if (string.Equals(nameof(DdsRedirectRule.OldPattern), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.OldPattern) :
                            redirectRules.OrderBy(o => o.OldPattern);
                        continue;
                    }

                    if (string.Equals(nameof(DdsRedirectRule.NewPattern), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.NewPattern) :
                            redirectRules.OrderBy(o => o.NewPattern);
                        continue;
                    }

                    if (string.Equals(nameof(DdsRedirectRule.RedirectRuleType), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.RedirectRuleType) :
                            redirectRules.OrderBy(o => o.RedirectRuleType);
                        continue;
                    }

                    if (string.Equals(nameof(DdsRedirectRule.RedirectType), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.RedirectType) :
                            redirectRules.OrderBy(o => o.RedirectType);
                        continue;
                    }

                    if (string.Equals(nameof(DdsRedirectRule.RedirectOrigin), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.RedirectOrigin) :
                            redirectRules.OrderBy(o => o.RedirectOrigin);
                        continue;
                    }

                    if (string.Equals(nameof(DdsRedirectRule.CreatedOn), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.CreatedOn) :
                            redirectRules.OrderBy(o => o.CreatedOn);
                        continue;
                    }

                    if (string.Equals(nameof(DdsRedirectRule.IsActive), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.IsActive) :
                            redirectRules.OrderBy(o => o.IsActive);
                        continue;
                    }

                    if (string.Equals(nameof(DdsRedirectRule.CreatedBy), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.CreatedBy) :
                            redirectRules.OrderBy(o => o.CreatedBy);
                        continue;
                    }

                    if (string.Equals(nameof(DdsRedirectRule.Priority), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectRules = column.SortDescending ?
                            redirectRules.OrderByDescending(o => o.Priority) :
                            redirectRules.OrderBy(o => o.Priority);
                        continue;
                    }
                }
            }

            allRedirectsCount = redirectRules.Count();

            return redirectRules
                .ApplyRange(query.Range)
                .Items
                .AsEnumerable();
        }
    }
}
