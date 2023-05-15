using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Web;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Tests.Data;
using Moq;
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
            
            var mapper = new RedirectRuleModelMapper(options, new Mock<ISiteDefinitionRepository>().Object);
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
            
            Assert.Equal(redirectRuleDto.HostId, redirectRule.HostId);
        }

        [Fact]
        public void Given_RedirectRuleDTO_Map_ReturnsRedirectRuleWithValidHostId()
        {
            var options = new RedirectsOptions
            {
                DefaultRedirectRulePriority = 100,
            };
            
            var mapper = new RedirectRuleModelMapper(options, new Mock<ISiteDefinitionRepository>().Object);
            var redirectRuleDto = RandomDataGenerator.CreateRandomRedirectRuleDto();
            redirectRuleDto.HostId = Guid.NewGuid();
            var redirectRule = mapper.DtoToModel(redirectRuleDto);
            
            Assert.Equal(redirectRuleDto.HostId, redirectRule.HostId);
        }

        [Fact]
        public void Given_RedirectRule_Map_ReturnsRedirectRuleDto()
        {
            var options = new RedirectsOptions
            {
                DefaultRedirectRulePriority = 100,
            };
            
            var siteDefinitionRepository =  new Mock<ISiteDefinitionRepository>();
            siteDefinitionRepository.Setup(s => s.List()).Returns(Enumerable.Empty<SiteDefinition>());

            var mapper = new RedirectRuleModelMapper(options,  siteDefinitionRepository.Object);
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
            
            Assert.Equal(redirectRule.HostId, redirectRuleDto.HostId);
            Assert.Equal("All hosts", redirectRuleDto.HostName);
        }

        [Fact]
        public void Given_RedirectRule_Map_ReturnsRedirectRuleDtoWithValidHostIdAndName()
        {
            var options = new RedirectsOptions
            {
                DefaultRedirectRulePriority = 100,
            };
            
            var siteDefinitionRepository =  new Mock<ISiteDefinitionRepository>();
            const string hostId = "2c62ad9b-a5a5-413b-bf05-83f583dddab4";
            const string hostName = "Kongsberg first test site name";
            var siteDefinitions = new List<SiteDefinition>()
            {
                new SiteDefinition()
                    { Id = Guid.Parse(hostId), Name = hostName },
                new SiteDefinition()
                    { Id = Guid.Parse("5476fd14-7f34-4c66-a7d7-8e7ba504495c"), Name = "Kongsberg second test site name" },
                new SiteDefinition()
                    { Id = Guid.Parse("898d7167-36f4-497e-bddb-ac62a5ba954f"), Name = "Kongsberg third test site name"}
            };
            siteDefinitionRepository.Setup(s => s.List()).Returns(siteDefinitions);

            var mapper = new RedirectRuleModelMapper(options, siteDefinitionRepository.Object);
            var redirectRule = RandomDataGenerator.CreateRandomRedirectRule();
            redirectRule.HostId = Guid.Parse(hostId);
            var redirectRuleDto = mapper.ModelToDto(redirectRule);
            var expectedSiteDefinitionName = siteDefinitions.Where(s => s.Id == redirectRule.HostId).Select(s=> s.Name).FirstOrDefault();
            
            Assert.Equal(redirectRule.HostId, redirectRuleDto.HostId);
            Assert.Equal(expectedSiteDefinitionName, redirectRuleDto.HostName);
            
        }
    }
}