using System;
using System.Linq;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Repository;

namespace Forte.RedirectMiddleware.Service
{
    [ServiceConfiguration(ServiceType = typeof(IRedirectService))]
    public class RedirectService : IRedirectService
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;

        public RedirectService(IRedirectRuleRepository redirectRuleRepository)
        {
            _redirectRuleRepository = redirectRuleRepository;
        }

        public RedirectRule GetRedirectRule(string oldPath)
        {
            var urlPath = UrlPath.Create(oldPath);
            return _redirectRuleRepository.GetRedirectRule(urlPath);
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

    public interface IRedirectService
    {
        RedirectRule GetRedirectRule(string oldPath);
        IQueryable<RedirectRule> GetAllRedirectRules();
        RedirectRuleDto CreateRedirect(RedirectRuleDto redirectRuleDto);
        RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectRuleDto);
        bool DeleteRedirect(Guid id);
    }
}