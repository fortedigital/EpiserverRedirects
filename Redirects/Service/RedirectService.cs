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
        private readonly IRedirectTypeMapper _redirectTypeMapper;
        public RedirectService(IRedirectRuleRepository redirectRuleRepository)
        {
            _redirectRuleRepository = redirectRuleRepository;
        }

        public RedirectRuleDto GetRedirect(string oldPath)
        {
            var urlPath = new UrlPath(oldPath);
            return _redirectRuleRepository.GetRedirect(urlPath);
        }
        
        public IQueryable<RedirectRuleDto> GetAllRedirects()
        {
            return _redirectRuleRepository.GetAllRedirects();
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
        RedirectRuleDto GetRedirect(string oldPath);
        IQueryable<RedirectRuleDto> GetAllRedirects();
        RedirectRuleDto CreateRedirect(RedirectRuleDto redirectVM);
        RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectVM);
        bool DeleteRedirect(Guid id);
    }
}