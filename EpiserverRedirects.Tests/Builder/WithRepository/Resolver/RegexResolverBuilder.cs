using EPiServer;
using Forte.EpiserverRedirects.Resolver;
using Forte.EpiserverRedirects.Resolver.Content;
using Moq;

namespace Forte.EpiserverRedirects.Tests.Builder.WithRepository.Resolver
{
    public class RegexResolverBuilder : BaseWithRepositoryBuilder<RegexResolver, RegexResolverBuilder>
    {
        protected override RegexResolverBuilder ThisBuilder => this;

        public override RegexResolver Create()
        {
            CreateRepository();
            return new RegexResolver(RedirectRuleRepository, new []{ new DefaultRedirectContentResolver(Mock.Of<IContentLoader>()) });
        }
    }
}
