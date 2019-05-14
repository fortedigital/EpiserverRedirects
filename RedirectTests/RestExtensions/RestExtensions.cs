using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Xunit;

namespace RedirectTests.RestExtensions
{
    public static class RestExtensions
    {
        private static RestResult ToRestResult(ActionResult actionResult)
        {
            return Assert.IsType<RestResult>(actionResult);
        }

        public static HttpStatusCode GetStatusCodeFromActionResult(this ActionResult actionResult)
        {
            var restResult = ToRestResult(actionResult);
            return Assert.IsType<HttpStatusCode>(restResult.Data);
        }
        
        public static RedirectRuleDto GetEntityFromActionResult(this ActionResult actionResult)
        {
            var restResult = ToRestResult(actionResult);
            return Assert.IsType<RedirectRuleDto>(restResult.Data);
        }
        
        public static IEnumerable<RedirectRuleDto> GetEntitiesFromActionResult(this ActionResult actionResult)
        {
            var restResult = ToRestResult(actionResult);
            return (IEnumerable<RedirectRuleDto>)restResult.Data;
        }
    }
}