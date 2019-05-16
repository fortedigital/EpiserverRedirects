using Forte.Redirects.Resolver;

namespace Forte.RedirectTests.Builder.WithRepository.Resolver
{
    public class ExactMatchResolverBuilder : BaseWithRepositoryBuilder<ExactMatchResolver, ExactMatchResolverBuilder>
    {
        protected override ExactMatchResolverBuilder ThisBuilder => this;

        public override ExactMatchResolver Create()
        {
            CreateRepository();
            return new ExactMatchResolver(RedirectRuleRepository);
        }
    }
}