using Forte.Redirects.Mapper;
using Forte.Redirects.Model.UrlPath;
using RedirectTests.Data;
using Xunit;

namespace RedirectTests.Tests.Mapper
{
    public class MapperTests
    {
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