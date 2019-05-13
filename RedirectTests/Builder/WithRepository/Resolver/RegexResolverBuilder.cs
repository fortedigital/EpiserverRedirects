using Forte.RedirectMiddleware.Resolver.Regex;

namespace RedirectTests.Builder.WithRepository.Resolver
{
    public class RegexResolverBuilder : BaseWithRepositoryBuilder<RegexResolver, RegexResolverBuilder>
    {
        protected override RegexResolverBuilder ThisBuilder => this;

        public override RegexResolver Create()
        {
            CreateRepository();
            return new RegexResolver(RedirectRuleRepository);
        }
    }
}