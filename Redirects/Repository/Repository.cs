using System;
using System.Collections.Generic;
using System.Linq;
using Forte.RedirectMiddleware.Model;

namespace Forte.RedirectMiddleware.Repository
{
    public interface IRedirectRuleRepository
    {
        RedirectRule GetRedirectRule(UrlPath oldPath);
        IEnumerable<RedirectRule> GetAllRedirectRules();
        RedirectRule GetRedirectRule(Guid id);
        RedirectRule CreateRedirect(RedirectRule redirectRule);
        RedirectRule UpdateRedirect(RedirectRule redirectRule);
        bool DeleteRedirect(Guid id);
    }

    public abstract class RedirectRuleRepository : IRedirectRuleRepository
    {
        public abstract RedirectRule GetRedirectRule(UrlPath oldPath);
        public abstract IEnumerable<RedirectRule> GetAllRedirectRules();
        public abstract RedirectRule GetRedirectRule(Guid id);

        public abstract RedirectRule CreateRedirect(RedirectRule redirectRule);
        public abstract RedirectRule UpdateRedirect(RedirectRule redirectRule);
        public abstract bool DeleteRedirect(Guid id);

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