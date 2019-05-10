using Forte.RedirectMiddleware.Resolver.Regex;

namespace RedirectTests.Tests.Builder.Resolver
{
    public class RegexResolverBuilder : BaseBuilder<RegexResolver, RegexResolverBuilder>
    {
        protected override RegexResolverBuilder ThisBuilder => this;

        public override RegexResolver Create()
        {
            CreateRepository();
            return new RegexResolver(RedirectRuleRepository);
        }
    }
}