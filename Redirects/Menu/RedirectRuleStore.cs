using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EPiServer.Security;
using EPiServer.Shell.Services.Rest;
using Forte.Redirects.Mapper;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Repository;

namespace Forte.Redirects.Menu
{
    [RestStore("RedirectRuleStore")]
    public class RedirectRuleStore : RestControllerBase
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly IRedirectRuleMapper _redirectRuleMapper;

        public RedirectRuleStore(IRedirectRuleRepository redirectRuleRepository, IRedirectRuleMapper redirectRuleMapper)
        {
            _redirectRuleRepository = redirectRuleRepository;
            _redirectRuleMapper = redirectRuleMapper;
        }

        public ActionResult Get(Guid id)
        {
            var redirect = _redirectRuleRepository.GetById(id);

            if (redirect == null)
                return null;
            
            return Rest(_redirectRuleMapper.ModelToDto(redirect));
        }
        
        [HttpGet]
        public ActionResult Get(Query query = null)
        {
            var redirects = _redirectRuleRepository
                .Get(query)
                .Select(_redirectRuleMapper.ModelToDto);
            
            return Rest(redirects);
        }

        [HttpPost]
        public ActionResult Post(RedirectRuleDto dto)
        {
            if (!ViewData.ModelState.IsValid)
                return null;
            
            var newRedirectRule = _redirectRuleMapper.DtoToModel(dto);

            newRedirectRule.FromManual();
            
            newRedirectRule = _redirectRuleRepository.Add(newRedirectRule);

            var newRedirectRuleDto = _redirectRuleMapper.ModelToDto(newRedirectRule);
            return Rest(newRedirectRuleDto);
        }

        [HttpPut]
        public ActionResult Put(RedirectRuleDto dto)
        {
            if (!ViewData.ModelState.IsValid)
                return null;
            
            var updatedRedirectRule = _redirectRuleMapper.DtoToModel(dto);
            updatedRedirectRule = _redirectRuleRepository.Update(updatedRedirectRule);
            
            var updatedRedirectRuleDto = _redirectRuleMapper.ModelToDto(updatedRedirectRule);
            return Rest(updatedRedirectRuleDto);
        }
        
        [HttpDelete]
        public ActionResult Delete(Guid id)
        {
            var deletedSuccessfully = _redirectRuleRepository.Delete(id);
            return deletedSuccessfully
                ? Rest(HttpStatusCode.OK)
                : Rest(HttpStatusCode.Conflict);
        }
    }

}