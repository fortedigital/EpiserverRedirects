using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Tests.Data;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Tests
{
    public class MapperTests
    {
        [Fact]
        public void Given_RedirectRuleDTO_Map_ReturnsRedirectRule()
        {
            var options = new RedirectsOptions
            {
                DefaultRedirectRulePriority = 100,
            };

            var mapper = new RedirectRuleModelMapper(options);
            var redirectRuleDto = RandomDataGenerator.CreateRandomRedirectRuleDto();
            var redirectRule = mapper.DtoToModel(redirectRuleDto);

            Assert.Equal(redirectRuleDto.Id, redirectRule.RuleId);
            Assert.Equal(UrlPath.NormalizePath(redirectRuleDto.OldPattern), redirectRule.OldPattern);
            Assert.Equal(UrlPath.NormalizePath(redirectRuleDto.NewPattern), redirectRule.NewPattern);

            Assert.Equal(redirectRuleDto.RedirectType, redirectRule.RedirectType);
            Assert.Equal(redirectRuleDto.RedirectRuleType, redirectRule.RedirectRuleType);

            Assert.Equal(redirectRuleDto.IsActive, redirectRule.IsActive);
            Assert.Equal(redirectRuleDto.Notes, redirectRule.Notes);

            Assert.Equal(redirectRuleDto.Priority, redirectRule.Priority);
        }

        [Fact]
        public void Given_RedirectRule_Map_ReturnsRedirectRuleDto()
        {
            var options = new RedirectsOptions
            {
                DefaultRedirectRulePriority = 100,
            };

            var mapper = new RedirectRuleModelMapper(options);
            var redirectRule = RandomDataGenerator.CreateRandomRedirectRule();
            var redirectRuleDto = mapper.ModelToDto(redirectRule);

            Assert.Equal(redirectRule.RuleId, redirectRuleDto.Id);
            Assert.Equal(redirectRule.OldPattern, redirectRuleDto.OldPattern);
            Assert.Equal(redirectRule.NewPattern, redirectRuleDto.NewPattern);

            Assert.Equal(redirectRule.RedirectType, redirectRuleDto.RedirectType);
            Assert.Equal(redirectRule.RedirectRuleType, redirectRuleDto.RedirectRuleType);
            Assert.Equal(redirectRule.RedirectOrigin, redirectRuleDto.RedirectOrigin);
            
            Assert.Equal(redirectRuleDto.CreatedOn, redirectRule.CreatedOn);
            Assert.Equal(redirectRuleDto.CreatedBy, redirectRule.CreatedBy);
            
            Assert.Equal(redirectRule.IsActive, redirectRuleDto.IsActive);
            Assert.Equal(redirectRule.Notes, redirectRuleDto.Notes);

            Assert.Equal(redirectRuleDto.Priority, redirectRule.Priority);
        }
    }
}