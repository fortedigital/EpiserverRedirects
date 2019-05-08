namespace RedirectTests.Tests.Builder.Resolver
{
    public class ExactMatchResolverBuilder : BaseBuilder<Forte.RedirectMiddleware.Resolver.ExactMatchResolver, ExactMatchResolverBuilder>
    {
        protected override ExactMatchResolverBuilder ThisBuilder => this;

        public override Forte.RedirectMiddleware.Resolver.ExactMatchResolver Create()
        {
            CreateRepository();
            return new Forte.RedirectMiddleware.Resolver.ExactMatchResolver(RedirectRuleRepository);
        }
    }
}