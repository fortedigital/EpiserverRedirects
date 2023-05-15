using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


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

            if (query.HostId != HostStore.AllDto.Id && query.HostId is not null)
            {
                rules = query.HostId == HostStore.AllHostDto.Id ? rules.Where(rr => rr.HostId == null) : rules.Where(rr => rr.HostId == query.HostId);
            }

            rules = ApplySorting(rules, query.SortColumns);
            allRedirectsCount = rules.Count();
            return rules
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

            var first = true;
            foreach (var sortColumn in sortColumns)
            {
                rules = OrderBy(rules, sortColumn, first);
                first = false;
            }

            return rules;
        }

        private static IQueryable<IRedirectRule> OrderBy(
            IQueryable<IRedirectRule> rules,
            SortColumn column,
            bool first)
        {
            var parameterExpression = Expression.Parameter(typeof(IRedirectRule));
            var propertyExpression = Expression.Property(parameterExpression, column.ColumnName);

            var methodName = first
                ? column.SortDescending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy)
                : column.SortDescending ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy);

            var expression = (Expression)Expression.Call(
                typeof(Queryable),
                methodName,
                new[]
                {
                    typeof(IRedirectRule),
                    propertyExpression.Type
                },
                rules.Expression,
                Expression.Quote(Expression.Lambda(propertyExpression, parameterExpression)));

            return (IQueryable<IRedirectRule>)rules.Provider.CreateQuery(expression);
        }
    }
}
