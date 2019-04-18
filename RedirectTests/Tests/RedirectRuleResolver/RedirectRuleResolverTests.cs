using Forte.RedirectMiddleware.Model;
using Xunit;

namespace RedirectTests.Tests.RedirectRuleResolver
{
    public class RedirectRuleResolverTests
    {
        private static RedirectRuleResolverBuilder RedirectRuleResolver() => new RedirectRuleResolverBuilder();

        [Fact]
        public void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .Create();
            
            var resolvedRule = resolver.ResolveRedirectRule(UrlPath.Create("/dummyPath"));
            
            Assert.Null(resolvedRule);
        }

        [Fact]
        public void Given_NonMatchingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .Create();
            
            var resolvedRule = resolver.ResolveRedirectRule(UrlPath.Create("/dummyPath"));
            
            Assert.Null(resolvedRule);
        }
        
        [Fact]
        public void Given_MatchingRule_Resolve_ReturnsTheRule()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .WithRule(r => r.WithOldPath("/dummyPath"), out var expectedRule)
                .Create();
            
            var resolvedRule = resolver.ResolveRedirectRule(expectedRule.OldPath);
            
            Assert.StrictEqual(expectedRule.Id, resolvedRule.Id);
        }
           
        /// <summary>
        /// /
        /// </summary>
        [Fact]
        public void Given_MatchingRule_Resolve_ReturnsRuleWithCorrectPath()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules()
                .WithRule(r=>r.WithOldPathAndNewUrl("/oldPath2", "/newUrl2"), out var expectedRule)
                .Create();
            
            var resolvedRule = resolver.ResolveRedirectRule(expectedRule.OldPath);
            
            Assert.Equal(expectedRule.NewUrl, resolvedRule.NewUrl);
        }
        
    }
}