using Forte.RedirectMiddleware.Repository;

namespace RedirectTests.Tests.RedirectRuleResolver
{
    public class RedirectRuleResolverBuilder : BaseBuilder<Forte.RedirectMiddleware.Resolver.RedirectRuleResolver>
    {
        public override Forte.RedirectMiddleware.Resolver.RedirectRuleResolver Create()
        {
            var existingRules = RedirectRuleTestDataBuilder.GetData();
            RedirectRuleResolverRepository = new TestRepository(existingRules);
            return new Forte.RedirectMiddleware.Resolver.RedirectRuleResolver(RedirectRuleResolverRepository);
        }
    }
}