using EPiServer;
using Forte.EpiserverRedirects.Resolver;
using Forte.EpiserverRedirects.Resolver.Content;
using Moq;

namespace Forte.EpiserverRedirects.Tests.Builder.WithRepository.Resolver
{
    public class ExactMatchResolverBuilder : BaseWithRepositoryBuilder<ExactMatchResolver, ExactMatchResolverBuilder>
    {
        protected override ExactMatchResolverBuilder ThisBuilder => this;

        public override ExactMatchResolver Create()
        {
            CreateRepository();
            return new ExactMatchResolver(RedirectRuleRepository, new []{ new DefaultRedirectContentResolver(Mock.Of<IContentLoader>()) });
        }
    }
}
