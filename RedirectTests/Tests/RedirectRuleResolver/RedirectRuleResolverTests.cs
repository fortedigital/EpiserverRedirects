using System.Threading.Tasks;
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
        
        [Fact]
        public void Given_RedirectRuleDTO_Map_ReturnsRedirectRule()
        {
            var mapper = new RedirectRuleMapper();
            var redirectRuleDto = RandomDataGenerator.CreateRandomRedirectRuleDto();
            var redirectRule = mapper.DtoToModel(redirectRuleDto);
            
            Assert.Equal(redirectRuleDto.Id, redirectRule.Id);
            Assert.Equal(UrlPath.NormalizePath(redirectRuleDto.Pattern), redirectRule.OldPath.Path.OriginalString);
            Assert.Equal(redirectRuleDto.NewUrl, redirectRule.NewPattern);
            Assert.Equal(redirectRuleDto.Notes, redirectRule.Notes);
            Assert.Equal(redirectRuleDto.IsActive, redirectRule.IsActive);
            Assert.Equal(redirectRuleDto.CreatedOn, redirectRule.CreatedOn);
            Assert.Equal(redirectRuleDto.RedirectType, redirectRule.RedirectType);
            Assert.Equal(redirectRuleDto.CreatedBy, redirectRule.CreatedBy);
            Assert.Equal(redirectRuleDto.RedirectRuleType, redirectRule.RedirectRuleType);
        }
        
        [Fact]
        public void Given_RedirectRule_Map_ReturnsRedirectRuleDto()
        {
            var mapper = new RedirectRuleMapper();
            var redirectRule = RandomDataGenerator.CreateRandomRedirectRule();
            var redirectRuleDto = mapper.ModelToDto(redirectRule);
            
            Assert.Equal(redirectRule.Id, redirectRuleDto.Id);
            Assert.Equal(redirectRule.OldPath.Path.OriginalString, redirectRuleDto.Pattern);
            Assert.Equal(redirectRule.NewPattern, redirectRuleDto.NewUrl);
            Assert.Equal(redirectRule.Notes, redirectRuleDto.Notes);
            Assert.Equal(redirectRule.IsActive, redirectRuleDto.IsActive);
            Assert.Equal(redirectRule.CreatedOn, redirectRuleDto.CreatedOn);
            Assert.Equal(redirectRule.RedirectType, redirectRuleDto.RedirectType);
            Assert.Equal(redirectRule.CreatedBy, redirectRuleDto.CreatedBy);
            Assert.Equal(redirectRule.RedirectRuleType, redirectRuleDto.RedirectRuleType);
        }
        
    }
}