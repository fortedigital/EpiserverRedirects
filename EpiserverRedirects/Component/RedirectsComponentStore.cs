using System.Linq;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Component
{
    [RestStore("RedirectsComponentStore")]
    public class RedirectsComponentStore : RestControllerBase
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly IRedirectRuleMapper _redirectRuleMapper;

        public RedirectsComponentStore(IRedirectRuleRepository redirectRuleRepository, IRedirectRuleMapper redirectRuleMapper)
        {
            _redirectRuleRepository = redirectRuleRepository;
            _redirectRuleMapper = redirectRuleMapper;
        }

        [HttpGet]
        public ActionResult Get(int contentId, string filter)
        {
            var result = _redirectRuleRepository
                .GetAll()
                .Where(item => item.OldPattern.Contains(filter))
                .Where(item => item.ContentId == contentId);

            return Rest(result.ToList());
        }
    }
}