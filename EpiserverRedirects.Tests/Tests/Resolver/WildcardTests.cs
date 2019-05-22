using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Tests.Builder.WithRepository.Resolver;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Tests.Resolver
{
    public class WildcardTests
    {
        private static WildcardResolverBuilder WildcardResolver() => new WildcardResolverBuilder();

        [Fact]
        public async void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = WildcardResolver()
                .Create();
            
            var redirect = await resolver.ResolveRedirectRule(UrlPath.Parse("/dummyPath"));
            Assert.Null(redirect?.Id);
        }
    }
}