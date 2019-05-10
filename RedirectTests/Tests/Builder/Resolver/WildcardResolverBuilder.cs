using Forte.RedirectMiddleware.Resolver.Wildcard;

namespace RedirectTests.Tests.Builder.Resolver
{
    public class WildcardResolverBuilder : BaseBuilder<WildcardResolver, WildcardResolverBuilder>
    {
        protected override WildcardResolverBuilder ThisBuilder => this;

        public override WildcardResolver Create()
        {
            CreateRepository();
            return new WildcardResolver(RedirectRuleRepository);
        }
    }
}