using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Repository.ControllerRepository
{
    public interface IRedirectRuleControllerRepository
    {
        IQueryable<RedirectRule> Get();
        RedirectRule GetById(Guid id);
        RedirectRule Add(RedirectRule redirectRule);
        RedirectRule Update(RedirectRule redirectRule);
        bool Delete(Guid id);
    }

    public abstract class RedirectRuleControllerRepository : IRedirectRuleControllerRepository
    {
        public abstract IQueryable<RedirectRule> Get();
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
    }
}