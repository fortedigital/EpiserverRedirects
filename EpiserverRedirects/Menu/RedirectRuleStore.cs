using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Forte.EpiserverRedirects.Menu
{
    [RestStore("RedirectRuleStore")]
    public class RedirectRuleStore : RestControllerBase
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly IRedirectRuleModelMapper _redirectRuleMapper;
        private readonly Guid _clearAllGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");

        public RedirectRuleStore(IRedirectRuleRepository redirectRuleRepository, IRedirectRuleModelMapper redirectRuleMapper)
        {
            _redirectRuleRepository = redirectRuleRepository;
            _redirectRuleMapper = redirectRuleMapper;
        }

        public ActionResult Get(Guid id)
        {
            var redirect = _redirectRuleRepository.GetById(id);

            return redirect == null ? null : Rest(_redirectRuleMapper.ModelToDto(redirect));
        }

        [HttpGet]
        public ActionResult Get(Query query = null)
        {
            var redirects = _redirectRuleRepository
                .GetAll()
                .GetPageFromQuery(out var allRedirectsCount, query)
                .Select(_redirectRuleMapper.ModelToDto);

            var itemRange = new ItemRange
            {
                Total = allRedirectsCount
            };

            itemRange.AddHeaderTo(HttpContext.Response);
            return Rest(redirects);
        }

        public ActionResult Post(RedirectRulesDto request)
        {
            if (!ViewData.ModelState.IsValid)
            {
                return null;
            }

            return request.Operation switch
            {
                RedirectRuleOperation.Create => AddRedirectRule(request.RedirectRules),
                RedirectRuleOperation.Update => UpdateRedirectRule(request.RedirectRules),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private ActionResult AddRedirectRule(IEnumerable<RedirectRuleDto> dtos)
        {
            var newRedirectRules = dtos.Select(d =>
            {
                var result = _redirectRuleMapper.DtoToModel(d);
                RedirectRuleModel.FromManual(result);
                return result;
            });
            newRedirectRules = _redirectRuleRepository.AddRange(newRedirectRules);
            var newRedirectRuleDtos = newRedirectRules.Select(_redirectRuleMapper.ModelToDto);

            return Rest(newRedirectRuleDtos);
        }

        private ActionResult UpdateRedirectRule(IEnumerable<RedirectRuleDto> dtos)
        {
            var updatedRedirectRules = dtos.Select(_redirectRuleMapper.DtoToModel);
            updatedRedirectRules = _redirectRuleRepository.UpdateRange(updatedRedirectRules);
            var updatedRedirectRuleDtos = updatedRedirectRules.Select(_redirectRuleMapper.ModelToDto) ;
            
            return Rest(updatedRedirectRuleDtos);
        }

        [HttpDelete]
        public ActionResult Delete(Guid id)
        {
            var deletedSuccessfully = id == _clearAllGuid
                ? _redirectRuleRepository.ClearAll()
                : _redirectRuleRepository.Delete(id);

            return deletedSuccessfully
                ? Rest(HttpStatusCode.OK)
                : Rest(HttpStatusCode.Conflict);
        }
    }
}
