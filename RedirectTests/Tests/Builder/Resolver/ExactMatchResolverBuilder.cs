using Forte.RedirectMiddleware.Resolver.ExactMatch;

namespace RedirectTests.Tests.Builder.Resolver
{
    public class ExactMatchResolverBuilder : BaseBuilder<ExactMatchResolver, ExactMatchResolverBuilder>
    {
        protected override ExactMatchResolverBuilder ThisBuilder => this;

        public override ExactMatchResolver Create()
        {
            CreateRepository();
            return new ExactMatchResolver(RedirectRuleRepository);
        }
    }
}