using System;
using System.Linq;
using Forte.RedirectMiddleware.Model;

namespace Forte.RedirectMiddleware.Repository
{
    public interface IRedirectRuleRepository
    {
        RedirectRule GetRedirectRule(UrlPath oldPath);
        IQueryable<RedirectRule> GetAllRedirectRules();
        RedirectRuleDto CreateRedirect(RedirectRuleDto redirectVM);
        RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectVM);
        bool DeleteRedirect(Guid id);
    }

    public abstract class RedirectRuleRepository : IRedirectRuleRepository
    {
        public abstract RedirectRule GetRedirectRule(UrlPath oldPath);
        public abstract IQueryable<RedirectRule> GetAllRedirectRules();

        public abstract RedirectRuleDto CreateRedirect(RedirectRuleDto redirectRuleDTO);
        public abstract RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectRuleDTO);
        public abstract bool DeleteRedirect(Guid id);
    }
}