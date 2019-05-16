using System.Linq;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.Redirects.Mapper;
using Forte.Redirects.Repository;

namespace Forte.Redirects.Component
{
    [RestStore("UrlRedirectsComponentStore")]
    public class UrlRedirectsComponentStore : RestControllerBase
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly IRedirectRuleMapper _redirectRuleMapper;

        public UrlRedirectsComponentStore(IRedirectRuleRepository redirectRuleRepository, IRedirectRuleMapper redirectRuleMapper)
        {
            _redirectRuleRepository = redirectRuleRepository;
            _redirectRuleMapper = redirectRuleMapper;
        }

        [HttpGet]
        public ActionResult Get(int contentId, string filter)
        {
            var result = _redirectRuleRepository
                .Where(item => item.OldPattern.Contains(filter))
                .Where(item => item.ContentId == contentId);

            return Rest(result.ToList());
        }
    }
}