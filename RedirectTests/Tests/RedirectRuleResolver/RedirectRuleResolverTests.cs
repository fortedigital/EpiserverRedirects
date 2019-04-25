using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.Mapper;
using Forte.RedirectMiddleware.Model.UrlPath;
using RedirectTests.Data;
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
            
            var resolvedRule = resolver.ResolveRedirectRule(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(resolvedRule);
        }

        [Fact]
        public void Given_NonMatchingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .Create();
            
            var resolvedRule = resolver.ResolveRedirectRule(UrlPath.Parse("/dummyPath"));
            
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
        /// przeniesc do testow repo lub usunac?
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
        
        [Fact]
        public void Given_RedirectRuleDTO_Map_ReturnsRedirectRule()
        {
            var mapper = new RedirectRuleMapper();
            var redirectRuleDto = RandomDataGenerator.CreateRandomRedirectRuleDto();
            var redirectRule = mapper.DtoToModel(redirectRuleDto);
            
            Assert.Equal(redirectRuleDto.Id, redirectRule.Id);
            Assert.Equal(redirectRuleDto.OldPath, redirectRule.OldPath.Path.OriginalString);
            Assert.Equal(redirectRuleDto.NewUrl, redirectRule.NewUrl);
            Assert.Equal(redirectRuleDto.Notes, redirectRule.Notes);
            Assert.Equal(redirectRuleDto.IsActive, redirectRule.IsActive);
            Assert.Equal(redirectRuleDto.CreatedOn, redirectRule.CreatedOn);
            Assert.Equal(redirectRuleDto.RedirectType, redirectRule.RedirectType);
        }
        
        [Fact]
        public void Given_RedirectRule_Map_ReturnsRedirectRuleDto()
        {
            var mapper = new RedirectRuleMapper();
            var redirectRule = RandomDataGenerator.CreateRandomRedirectRule();
            var redirectRuleDto = mapper.ModelToDto(redirectRule);
            
            Assert.Equal(redirectRule.Id, redirectRuleDto.Id);
            Assert.Equal(redirectRule.OldPath.Path.OriginalString, redirectRuleDto.OldPath);
            Assert.Equal(redirectRule.NewUrl, redirectRuleDto.NewUrl);
            Assert.Equal(redirectRule.Notes, redirectRuleDto.Notes);
            Assert.Equal(redirectRule.IsActive, redirectRuleDto.IsActive);
            Assert.Equal(redirectRule.CreatedOn, redirectRuleDto.CreatedOn);
            Assert.Equal(redirectRule.RedirectType, redirectRuleDto.RedirectType);
        }
        
    }
}