using System;
using System.Linq;
using EPiServer.Shell.Services.Rest;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Repository;

namespace Forte.RedirectMiddleware.REST
{
    public class RedirectRuleController : RestControllerBase
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;

        public RedirectRuleController(IRedirectRuleRepository redirectRuleRepository)
        {
            _redirectRuleRepository = redirectRuleRepository;
        }
        
        public IQueryable<RedirectRule> GetAllRedirectRules()
        {
            return _redirectRuleRepository.GetAllRedirectRules();
        }

        public RedirectRuleDto CreateRedirect(RedirectRuleDto redirectRuleDTO)
        {
            return RedirectRuleValidator.ValidateDto(redirectRuleDTO)
                ? _redirectRuleRepository.CreateRedirect(redirectRuleDTO)
                : null;
        }

        public RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectRuleDTO)
        {
            return RedirectRuleValidator.ValidateDto(redirectRuleDTO)
                ? _redirectRuleRepository.UpdateRedirect(redirectRuleDTO)
                : null;
        }
        
        public bool DeleteRedirect(Guid id)
        {
            return _redirectRuleRepository.DeleteRedirect(id);
        }
    }
}