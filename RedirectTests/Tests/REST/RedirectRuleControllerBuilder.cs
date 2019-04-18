using Forte.RedirectMiddleware.Repository;
using Forte.RedirectMiddleware.REST;

namespace RedirectTests.Tests.REST
{
    public class RedirectRuleControllerBuilder : BaseBuilder<RedirectRuleController>
    {
        public override RedirectRuleController Create()
        {
            var existingRules = RedirectRuleTestDataBuilder.GetData();
            RedirectRuleRepository = new TestRedirectRuleRepository(existingRules);
            return new RedirectRuleController(RedirectRuleRepository);
        }
    }
}