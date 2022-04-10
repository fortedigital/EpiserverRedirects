using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Tests.Builder.WithRepository.Resolver;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Tests.Resolver
{
    public class ExactMatchTests
    {
        private static ExactMatchResolverBuilder RedirectRuleResolver() => new ExactMatchResolverBuilder();

        [Fact]
        public async void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }

        [Fact]
        public async void Given_NonMatchingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }
        
        [Fact]
        public async void Given_MatchingRule_Resolve_ReturnsTheRule()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .WithRule(r => r.WithOldPath("/dummyPath"), out var expectedRule)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse(expectedRule.OldPattern));
            
            Assert.Equal(expectedRule.Id, redirect?.Id);
        }
    }
}