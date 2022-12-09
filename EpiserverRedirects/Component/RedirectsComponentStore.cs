using System.Linq;
using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Forte.EpiserverRedirects.Component
{
    [RestStore("RedirectsComponentStore")]
    public class RedirectsComponentStore : RestControllerBase
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;

        public RedirectsComponentStore(IRedirectRuleRepository redirectRuleRepository)
        {
            _redirectRuleRepository = redirectRuleRepository;
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