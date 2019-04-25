using Forte.RedirectMiddleware.Repository;
using Forte.RedirectMiddleware.Repository.ControllerRepository;
using Forte.RedirectMiddleware.Repository.ResolverRepository;

namespace RedirectTests.Tests.RedirectRuleResolver
{
    public class RedirectRuleResolverBuilder : BaseBuilder<Forte.RedirectMiddleware.Resolver.RedirectRuleResolver>
    {
        public override Forte.RedirectMiddleware.Resolver.RedirectRuleResolver Create()
        {
            var existingRules = RedirectRuleTestDataBuilder.GetData();
            RedirectRuleResolverRepository = new TestRedirectRuleResolverRepository(existingRules);
            return new Forte.RedirectMiddleware.Resolver.RedirectRuleResolver(RedirectRuleResolverRepository);
        }
    }
}