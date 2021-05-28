using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Menu
{
    [RestStore("RedirectRuleStore")]
    public class RedirectRuleStore : RestControllerBase
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly IRedirectRuleMapper _redirectRuleMapper;
        private readonly Guid _clearAllGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        private readonly Guid _clearAllDuplicatesGuid = Guid.Parse("00000000-0000-0000-0000-000000000001");

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
                .GetAll()
                .GetPageFromQuery(out var allRedirectsCount, query)
                .Select(_redirectRuleMapper.ModelToDto);
            
            HttpContext.Response.Headers.Add("Content-Range", $"0/{allRedirectsCount}");
            
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
            bool deletedSuccessfully;
            
            if (id == _clearAllGuid) {
                deletedSuccessfully = _redirectRuleRepository.ClearAll();
            } 
            else if(id == _clearAllDuplicatesGuid) {
                deletedSuccessfully = _redirectRuleRepository.RemoveAllDuplicates();
            }
            else {
                deletedSuccessfully = _redirectRuleRepository.Delete(id);
            }
            
            return deletedSuccessfully
                ? Rest(HttpStatusCode.OK)
                : Rest(HttpStatusCode.Conflict);
        }
    }

}
