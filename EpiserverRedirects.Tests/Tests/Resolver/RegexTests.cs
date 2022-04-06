using Forte.EpiserverRedirects.Tests.Builder.WithRepository.Resolver;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Tests.Resolver
{
    public class RegexTests
    {
        private static RegexResolverBuilder RegexResolver() => new RegexResolverBuilder();

        [Fact]
        public async void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = RegexResolver()
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }

        [Fact]
        public async void Given_NonMatchingRules_Resolve_ReturnsNull()
        {
            var resolver = RegexResolver()
                .WithRandomExistingRules(10)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }
        
        
        [Fact]
        public async void Given_MatchingRule_Resolve_ReturnsTheRule()
        {
            var resolver = RegexResolver()
                .WithRandomExistingRules()
                .WithRule(r=>r.WithOldPatternAndNewPattern("oldPattern", "newPattern/$1"), out var expectedRule)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/oldPattern"));

            Assert.Equal(expectedRule.Id, redirect?.Id);
        }
    }
}