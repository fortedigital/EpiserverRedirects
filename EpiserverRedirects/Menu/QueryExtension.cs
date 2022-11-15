using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Forte.EpiserverRedirects.Menu
{
    public static class QueryExtension
    {
        public static IEnumerable<IRedirectRule> GetPageFromQuery(this IQueryable<IRedirectRule> redirectRules,
            out int allRedirectsCount, Query query = null)
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

            redirectRules = ApplySorting(redirectRules, query.SortColumns);
            allRedirectsCount = redirectRules.Count();
            return redirectRules
                .ApplyRange(query.Range)
                .Items
                .AsEnumerable();
        }

        private static IQueryable<IRedirectRule> ApplySorting(IQueryable<IRedirectRule> rules, IEnumerable<SortColumn> sortColumns)
        {
            if (sortColumns == null)
            {
                return rules;
            }

            IOrderedQueryable<IRedirectRule> orderedRules = null;
            foreach (var column in sortColumns)
            {
                if (string.Equals(nameof(IRedirectRule.ContentId), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.ContentId);
                    continue;
                }

                if (string.Equals(nameof(IRedirectRule.OldPattern), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.OldPattern);
                    continue;
                }

                if (string.Equals(nameof(IRedirectRule.NewPattern), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.NewPattern);
                    continue;
                }

                if (string.Equals(nameof(IRedirectRule.RedirectRuleType), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.RedirectRuleType);
                    continue;
                }

                if (string.Equals(nameof(IRedirectRule.RedirectType), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.RedirectType);
                    continue;
                }

                if (string.Equals(nameof(IRedirectRule.RedirectOrigin), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.RedirectOrigin);
                    continue;
                }

                if (string.Equals(nameof(IRedirectRule.CreatedOn), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.CreatedOn);
                    continue;
                }

                if (string.Equals(nameof(IRedirectRule.IsActive), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.IsActive);
                    continue;
                }

                if (string.Equals(nameof(IRedirectRule.CreatedBy), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.CreatedBy);
                    continue;
                }

                if (string.Equals(nameof(IRedirectRule.Priority), column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderedRules = ApplySortField(rules, orderedRules, column, o => o.Priority);
                    continue;
                }
            }

            return orderedRules ?? rules;
        }

        private static IOrderedQueryable<IRedirectRule> ApplySortField<TKey>(
            IQueryable<IRedirectRule> rules,
            IOrderedQueryable<IRedirectRule> orderedRules,
            SortColumn column,
            Expression<Func<IRedirectRule, TKey>> keySelector)
        {
            if (orderedRules == null)
            {
                return column.SortDescending ?
                    rules.OrderByDescending(keySelector) :
                    rules.OrderBy(keySelector);
            }
            else
            {
                return column.SortDescending ?
                    orderedRules.ThenByDescending(keySelector) :
                    orderedRules.ThenBy(keySelector);
            }
        }
    }
}
