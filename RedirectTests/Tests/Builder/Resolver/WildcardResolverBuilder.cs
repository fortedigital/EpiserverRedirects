namespace RedirectTests.Tests.Builder.Resolver
{
    public class WildcardResolverBuilder : BaseBuilder<Forte.RedirectMiddleware.Resolver.WildcardResolver, WildcardResolverBuilder>
    {
        protected override WildcardResolverBuilder ThisBuilder => this;

        public override Forte.RedirectMiddleware.Resolver.WildcardResolver Create()
        {
            CreateRepository();
            return new Forte.RedirectMiddleware.Resolver.WildcardResolver(RedirectRuleRepository);
        }
    }
}