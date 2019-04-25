using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EPiServer.Shell.Services.Rest;
using Forte.RedirectMiddleware.Model.Mapper;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Repository;

namespace Forte.RedirectMiddleware.Controller
{
    public class RedirectRuleController : RestControllerBase
    {
        private readonly IRedirectRuleControllerRepository _redirectRuleControllerRepository;
        private readonly IRedirectRuleMapper _redirectRuleMapper;

        public RedirectRuleController(IRedirectRuleControllerRepository redirectRuleControllerRepository, IRedirectRuleMapper redirectRuleMapper)
        {
            _redirectRuleControllerRepository = redirectRuleControllerRepository;
            _redirectRuleMapper = redirectRuleMapper;
        }

        public RedirectRuleDto GetRedirect(Guid id)
        {
            var redirect = _redirectRuleControllerRepository.GetById(id);

            if (redirect == null)
                return null;
            
            return _redirectRuleMapper.ModelToDto(redirect);
        }
        public IEnumerable<RedirectRuleDto> GetAllRedirects()
        {
            return _redirectRuleControllerRepository.AsEnumerable().Select(_redirectRuleMapper.ModelToDto);
        }

        public RedirectRuleDto Add(RedirectRuleDto dto)
        {
            if (!ViewData.ModelState.IsValid)
                return null;
            
            var newRedirectRule = _redirectRuleMapper.DtoToModel(dto);
            newRedirectRule = _redirectRuleControllerRepository.Add(newRedirectRule);

            var newRedirectRuleDto = _redirectRuleMapper.ModelToDto(newRedirectRule);
            return newRedirectRuleDto;
        }

        public RedirectRuleDto Update(RedirectRuleDto dto)
        {
            if (!ViewData.ModelState.IsValid)
                return null;
            
            var updatedRedirectRule = _redirectRuleMapper.DtoToModel(dto);
            updatedRedirectRule = _redirectRuleControllerRepository.Update(updatedRedirectRule);
            
            var updatedRedirectRuleDto = _redirectRuleMapper.ModelToDto(updatedRedirectRule);
            return updatedRedirectRuleDto;
        }
        
        public bool Delete(Guid id)
        {
            return _redirectRuleControllerRepository.Delete(id);
        }
    }

}