using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Forte.Redirects.Model.RedirectRule;

namespace Forte.Redirects.Repository
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
        protected IQueryable<RedirectRule> Items { private get; set; }

        public abstract RedirectRule GetById(Guid id);

        public abstract RedirectRule Add(RedirectRule redirectRule);

        public abstract RedirectRule Update(RedirectRule redirectRule);

        public abstract bool Delete(Guid id);

        protected static void WriteToModel(RedirectRule redirectRule, RedirectRule redirectToUpdate)
        {
            redirectToUpdate.Id = redirectRule.Id;
            redirectToUpdate.NewPattern = redirectRule.NewPattern;
            redirectToUpdate.OldPath = redirectRule.OldPath;
            redirectToUpdate.RedirectType = redirectRule.RedirectType;
            redirectToUpdate.IsActive = redirectRule.IsActive;
            redirectToUpdate.Notes = redirectRule.Notes;
        }

        public IEnumerator<RedirectRule> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Items).GetEnumerator();
        }

        public Expression Expression => Items.Expression;

        public Type ElementType => Items.ElementType;

        public IQueryProvider Provider => Items.Provider;
    }
}