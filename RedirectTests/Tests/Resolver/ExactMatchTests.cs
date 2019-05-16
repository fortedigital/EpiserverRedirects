using Forte.Redirects.Model.UrlPath;
using RedirectTests.Builder.WithRepository.Resolver;
using Xunit;

namespace RedirectTests.Tests.Resolver
{
    public class ExactMatchTests
    {
        private static ExactMatchResolverBuilder RedirectRuleResolver() => new ExactMatchResolverBuilder();

        [Fact]
        public async void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .Create();
            
            var redirect = await resolver.ResolveRedirectRule(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }

        [Fact]
        public async void Given_NonMatchingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRule(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }
        
        [Fact]
        public async void Given_MatchingRule_Resolve_ReturnsTheRule()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .WithRule(r => r.WithOldPath("/dummyPath"), out var expectedRule)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRule(expectedRule.OldPath);
            
            Assert.Equal(expectedRule.Id, redirect?.Id);
        }
    }
}