using System;
using System.Linq;
using Forte.RedirectMiddleware.Model;

namespace Forte.RedirectMiddleware.Repository
{
    public interface IRedirectRuleRepository
    {
        RedirectRuleDto GetRedirect(UrlPath oldPath);
        IQueryable<RedirectRuleDto> GetAllRedirects();
        RedirectRuleDto CreateRedirect(RedirectRuleDto redirectVM);
        RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectVM);
        bool DeleteRedirect(Guid id);
    }

    public abstract class RedirectRuleRepository : IRedirectRuleRepository
    {
        public abstract RedirectRuleDto GetRedirect(UrlPath oldPath);
        public abstract IQueryable<RedirectRuleDto> GetAllRedirects();

        public abstract RedirectRuleDto CreateRedirect(RedirectRuleDto redirectRuleDTO);
        public abstract RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectRuleDTO);
        public abstract bool DeleteRedirect(Guid id);
    }
}