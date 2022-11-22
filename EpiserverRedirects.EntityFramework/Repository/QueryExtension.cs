using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.DynamicData;
using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System;
using System.Linq;


namespace Forte.EpiserverRedirects.EntityFramework.Repository
{
    public static class QueryExtension
    {
        public static SearchResult<RedirectRuleEntity> ApplyQuery(this IQueryable<RedirectRuleEntity> redirectRules,
            RedirectRuleQuery query = null)
        {
            if (query == null)
            {
                return new SearchResult<RedirectRuleEntity>
                {
                    Total = redirectRules.Count(),
                    Items = redirectRules.ToList()
                };
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
                IOrderedQueryable<RedirectRuleEntity> orderedRules = null;
                foreach (var column in query.SortColumns)
                {
                    if (string.Equals(nameof(DynamicDataRedirectRule.ContentId), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.ContentId) :
                                redirectRules.OrderBy(o => o.ContentId);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.ContentId) :
                                orderedRules.ThenBy(o => o.ContentId);
                        }

                        continue;
                    }

                    if (string.Equals(nameof(DynamicDataRedirectRule.OldPattern), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.OldPattern) :
                                redirectRules.OrderBy(o => o.OldPattern);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.OldPattern) :
                                orderedRules.ThenBy(o => o.OldPattern);
                        }

                        continue;
                    }

                    if (string.Equals(nameof(DynamicDataRedirectRule.NewPattern), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.NewPattern) :
                                redirectRules.OrderBy(o => o.NewPattern);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.NewPattern) :
                                orderedRules.ThenBy(o => o.NewPattern);
                        }

                        continue;
                    }

                    if (string.Equals(nameof(DynamicDataRedirectRule.RedirectRuleType), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.RedirectRuleType) :
                                redirectRules.OrderBy(o => o.RedirectRuleType);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.RedirectRuleType) :
                                orderedRules.ThenBy(o => o.RedirectRuleType);
                        }

                        continue;
                    }

                    if (string.Equals(nameof(DynamicDataRedirectRule.RedirectType), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.RedirectType) :
                                redirectRules.OrderBy(o => o.RedirectType);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.RedirectType) :
                                orderedRules.ThenBy(o => o.RedirectType);
                        }

                        continue;
                    }

                    if (string.Equals(nameof(DynamicDataRedirectRule.RedirectOrigin), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.RedirectOrigin) :
                                redirectRules.OrderBy(o => o.RedirectOrigin);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.RedirectOrigin) :
                                orderedRules.ThenBy(o => o.RedirectOrigin);
                        }

                        continue;
                    }

                    if (string.Equals(nameof(DynamicDataRedirectRule.CreatedOn), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.CreatedOn) :
                                redirectRules.OrderBy(o => o.CreatedOn);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.CreatedOn) :
                                orderedRules.ThenBy(o => o.CreatedOn);
                        }

                        continue;
                    }

                    if (string.Equals(nameof(DynamicDataRedirectRule.IsActive), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.IsActive) :
                                redirectRules.OrderBy(o => o.IsActive);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.IsActive) :
                                orderedRules.ThenBy(o => o.IsActive);
                        }

                        continue;
                    }

                    if (string.Equals(nameof(DynamicDataRedirectRule.CreatedBy), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.CreatedBy) :
                                redirectRules.OrderBy(o => o.CreatedBy);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.CreatedBy) :
                                orderedRules.ThenBy(o => o.CreatedBy);
                        }
                        continue;
                    }

                    if (string.Equals(nameof(DynamicDataRedirectRule.Priority), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (orderedRules == null)
                        {
                            orderedRules = column.SortDescending ?
                                redirectRules.OrderByDescending(o => o.Priority) :
                                redirectRules.OrderBy(o => o.Priority);
                        }
                        else
                        {
                            orderedRules = column.SortDescending ?
                                orderedRules.ThenByDescending(o => o.Priority) :
                                orderedRules.ThenBy(o => o.Priority);
                        }

                        continue;
                    }
                }

                if (orderedRules != null)
                {
                    redirectRules = orderedRules;
                }
            }

            return new SearchResult<RedirectRuleEntity>
            {
                Total = redirectRules.Count(),
                Items = redirectRules
                    .ApplyRange(query.Range)
                    .Items
                    .ToList()
            };
        }
    }
}
