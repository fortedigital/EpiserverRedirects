using Forte.RedirectMiddleware.Repository;

namespace RedirectTests.Tests.RedirectRuleResolver
{
    public class RedirectRuleResolverBuilder : BaseBuilder<Forte.RedirectMiddleware.Resolver.ExactMatchResolver>
    {
        public override Forte.RedirectMiddleware.Resolver.ExactMatchResolver Create()
        {
            var existingRules = RedirectRuleTestDataBuilder.GetData();
            RedirectRuleRepository = new TestRepository(existingRules);
            return new Forte.RedirectMiddleware.Resolver.ExactMatchResolver(RedirectRuleRepository);
        }
    }
}