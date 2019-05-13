using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EPiServer.Shell.Services.Rest;
using Forte.RedirectMiddleware.Mapper;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Repository;

namespace Forte.RedirectMiddleware.Controller
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
            var redirect = _redirectRuleRepository.GetById(id);

            if (redirect == null)
                return null;
            
            return _redirectRuleMapper.ModelToDto(redirect);
        }
        public IEnumerable<RedirectRuleDto> GetAllRedirects()
        {
            return _redirectRuleRepository.AsEnumerable().Select(_redirectRuleMapper.ModelToDto);
        }

        public RedirectRuleDto Add(RedirectRuleDto dto)
        {
            if (!ViewData.ModelState.IsValid)
                return null;
            
            var newRedirectRule = _redirectRuleMapper.DtoToModel(dto);
            newRedirectRule = _redirectRuleRepository.Add(newRedirectRule);

            var newRedirectRuleDto = _redirectRuleMapper.ModelToDto(newRedirectRule);
            return newRedirectRuleDto;
        }

        public RedirectRuleDto Update(RedirectRuleDto dto)
        {
            if (!ViewData.ModelState.IsValid)
                return null;
            
            var updatedRedirectRule = _redirectRuleMapper.DtoToModel(dto);
            updatedRedirectRule = _redirectRuleRepository.Update(updatedRedirectRule);
            
            var updatedRedirectRuleDto = _redirectRuleMapper.ModelToDto(updatedRedirectRule);
            return updatedRedirectRuleDto;
        }
        
        public bool Delete(Guid id)
        {
            return _redirectRuleRepository.Delete(id);
        }
    }

}