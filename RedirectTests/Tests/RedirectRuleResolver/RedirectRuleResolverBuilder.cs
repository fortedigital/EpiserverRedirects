using Forte.RedirectMiddleware.Repository;

namespace RedirectTests.Tests.RedirectRuleResolver
{
    public class RedirectRuleResolverBuilder : BaseBuilder<Forte.RedirectMiddleware.Service.RedirectRuleResolver>
    {
        public override Forte.RedirectMiddleware.Service.RedirectRuleResolver Create()
        {
            var existingRules = RedirectRuleTestDataBuilder.GetData();
            RedirectRuleRepository = new TestRedirectRuleRepository(existingRules);
            return new Forte.RedirectMiddleware.Service.RedirectRuleResolver(RedirectRuleRepository);
        }
    }
}