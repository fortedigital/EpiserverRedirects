using Forte.RedirectMiddleware.Model.UrlPath;
using RedirectTests.Tests.Builder.Resolver;
using Xunit;

namespace RedirectTests.Tests.Resolver
{
    public class RegexTests
    {
        private static RegexResolverBuilder RegexResolver() => new RegexResolverBuilder();

        [Fact]
        public async void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = RegexResolver()
                .Create();
            
            var redirect = await resolver.ResolveRedirectRule(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.RedirectRule);
        }

        [Fact]
        public async void Given_NonMatchingRules_Resolve_ReturnsNull()
        {
            var resolver = RegexResolver()
                .WithRandomExistingRules(10)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRule(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.RedirectRule);
        }
        
        
        [Fact]
        public async void Given_MatchingRule_Resolve_ReturnsTheRule()
        {
            var resolver = RegexResolver()
                .WithRandomExistingRules()
                .WithRule(r=>r.WithOldPatternAndNewPattern("oldPattern", "newPattern/$1"), out var expectedRule)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRule(UrlPath.Parse("/oldPattern"));

            Assert.Equal(expectedRule.Id, redirect?.RedirectRule.Id);
        }
    }
}