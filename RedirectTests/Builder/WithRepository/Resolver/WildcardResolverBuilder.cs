using Forte.Redirects.Resolver;

namespace Forte.RedirectTests.Builder.WithRepository.Resolver
{
    public class WildcardResolverBuilder : BaseWithRepositoryBuilder<WildcardResolver, WildcardResolverBuilder>
    {
        protected override WildcardResolverBuilder ThisBuilder => this;

        public override WildcardResolver Create()
        {
            CreateRepository();
            return new WildcardResolver(RedirectRuleRepository);
        }
    }
}