using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Repository
{
    public interface IRedirectRuleRepository : IQueryable<RedirectRule>
    {
        RedirectRule GetById(Guid id);
        RedirectRule Add(RedirectRule redirectRule);
        RedirectRule Update(RedirectRule redirectRule);
        bool Delete(Guid id);
    }

    public abstract class RedirectRuleRepository : IRedirectRuleRepository
    {
        public abstract RedirectRule GetById(Guid id);

        public abstract RedirectRule Add(RedirectRule redirectRule);
        public abstract RedirectRule Update(RedirectRule redirectRule);
        public abstract bool Delete(Guid id);

        protected static void WriteToModel(RedirectRule redirectRule, RedirectRule redirectToUpdate)
        {
            redirectToUpdate.Id = redirectRule.Id;
            redirectToUpdate.NewUrl = redirectRule.NewUrl;
            redirectToUpdate.OldPath = redirectRule.OldPath;
            redirectToUpdate.RedirectType = redirectRule.RedirectType;
            redirectToUpdate.IsActive = redirectRule.IsActive;
            redirectToUpdate.Notes = redirectRule.Notes;
        }

        public abstract IEnumerator<RedirectRule> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression => _expression;

        private Expression _expression;

        public Type ElementType => _elementType;

        private Type _elementType;
        public IQueryProvider Provider
        {
            get
            {
                InitQueryable();
                return _provider;
            }
        }

        private IQueryProvider _provider;

        protected abstract void InitQueryable();

        protected void InitQueryable(IQueryable queryable)
        {
            _expression = queryable.Expression;
            _elementType = queryable.ElementType;
            _provider = queryable.Provider;
        }
    }
}