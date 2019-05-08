using Forte.RedirectMiddleware.Model.UrlPath;
using RedirectTests.Tests.Builder;
using RedirectTests.Tests.Builder.Resolver;
using Xunit;

namespace RedirectTests.Tests.RedirectRuleResolver
{
    public class ExactMatchTests
    {
        private static ExactMatchResolverBuilder RedirectRuleResolver() => new ExactMatchResolverBuilder();

        [Fact]
        public async void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .Create();
            
            var resolvedRule = await resolver.ResolveRedirectRule(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(resolvedRule);
        }

        [Fact]
        public async void Given_NonMatchingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .Create();
            
            var resolvedRule = await resolver.ResolveRedirectRule(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(resolvedRule);
        }
        
        
        [Fact]
        public async void Given_MatchingRule_Resolve_ReturnsTheRule()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .WithRule(r => r.WithOldPath("/dummyPath"), out var expectedRule)
                .Create();
            
            var resolvedRule = await resolver.ResolveRedirectRule(expectedRule.OldPath);
            
            Assert.Equal(expectedRule.Id, resolvedRule.Id);
        }
           
        /// <summary>
        /// przeniesc do testow repo lub usunac?
        /// </summary>
        [Fact]
        public async void Given_MatchingRule_Resolve_ReturnsRuleWithCorrectPath()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules()
                .WithRule(r=>r.WithOldPathAndNewUrl("/oldPath2", "/newUrl2"), out var expectedRule)
                .Create();
            
            var resolvedRule = await resolver.ResolveRedirectRule(expectedRule.OldPath);
            
            Assert.Equal(expectedRule.NewPattern, resolvedRule.NewPattern);
        }
    }
}