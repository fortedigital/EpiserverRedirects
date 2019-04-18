using System;
using System.Collections.Generic;
using EPiServer.Shell.Services.Rest;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.Mapper;
using Forte.RedirectMiddleware.Repository;

namespace Forte.RedirectMiddleware.REST
{
    public class RedirectRuleController : RestControllerBase
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly IRedirectRuleMapper _redirectRuleMapper;

        public RedirectRuleController(IRedirectRuleRepository redirectRuleRepository, IRedirectRuleMapper redirectRuleMapper)
        {
            _redirectRuleRepository = redirectRuleRepository;
            _redirectRuleMapper = redirectRuleMapper;
        }

        public RedirectRuleDto GetRedirect(Guid id)
        {
            var redirect = _redirectRuleRepository.GetRedirectRule(id);
            var redirectDTO = _redirectRuleMapper.ModelToDto(redirect);
            return redirectDTO;
        }
        public IEnumerable<RedirectRuleDto> GetAllRedirects()
        {
            var redirects = _redirectRuleRepository.GetAllRedirectRules();
            var redirectsDTO = _redirectRuleMapper.ModelToDto(redirects);
            return redirectsDTO;
        }

        public RedirectRuleDto CreateRedirect(RedirectRuleDto redirectRuleDTO)
        {
            var newRedirectRule = _redirectRuleMapper.DtoToModel(redirectRuleDTO);
            newRedirectRule = RedirectRuleValidator.ValidateDto(redirectRuleDTO)
                ? _redirectRuleRepository.CreateRedirect(newRedirectRule)
                : null;

            var newRedirectRuleDto = _redirectRuleMapper.ModelToDto(newRedirectRule);
            return newRedirectRuleDto;
        }

        public RedirectRuleDto UpdateRedirect(RedirectRuleDto redirectRuleDTO)
        {
            var updatedRedirectRule = _redirectRuleMapper.DtoToModel(redirectRuleDTO);
            updatedRedirectRule = RedirectRuleValidator.ValidateDto(redirectRuleDTO)
                ? _redirectRuleRepository.UpdateRedirect(updatedRedirectRule)
                : null;
            
            var updatedRedirectRuleDto = _redirectRuleMapper.ModelToDto(updatedRedirectRule);
            return updatedRedirectRuleDto;
        }
        
        public bool DeleteRedirect(Guid id)
        {
            return _redirectRuleRepository.DeleteRedirect(id);
        }
    }

}