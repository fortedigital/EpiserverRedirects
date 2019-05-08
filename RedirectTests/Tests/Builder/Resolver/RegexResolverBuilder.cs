namespace RedirectTests.Tests.Builder.Resolver
{
    public class RegexResolverBuilder : BaseBuilder<Forte.RedirectMiddleware.Resolver.RegexResolver, RegexResolverBuilder>
    {
        protected override RegexResolverBuilder ThisBuilder => this;

        public override Forte.RedirectMiddleware.Resolver.RegexResolver Create()
        {
            CreateRepository();
            return new Forte.RedirectMiddleware.Resolver.RegexResolver(RedirectRuleRepository);
        }
    }
}