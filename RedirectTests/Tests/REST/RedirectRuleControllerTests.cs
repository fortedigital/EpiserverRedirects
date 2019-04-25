using System;
using System.Collections.Generic;
using System.Linq;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.Mapper;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using RedirectTests.Data;
using Xunit;

namespace RedirectTests.Tests.REST
{
    public class RedirectRuleControllerTests
    {
        private static RedirectRuleControllerBuilder RedirectRuleController() => new RedirectRuleControllerBuilder();
                
        [Fact]
        public void Given_ExistingRedirects_Controller_ReturnsAllRedirects()
        {
            var rule1 = RandomDataGenerator.CreateRandomRedirectRule();
            var rule2 = RandomDataGenerator.CreateRandomRedirectRule();
            
            var existingRules = new Dictionary<Guid, RedirectRule>
            {
                {rule1.Id.ExternalId, rule1},
                {rule2.Id.ExternalId, rule2}
            };
            var dto1 = new RedirectRuleDto();
            var dto2 = new RedirectRuleDto();
            
            var restController = RedirectRuleController()             
                .WithMapper(r => r == rule1 ? dto1 : r == rule2 ? dto2 : null)
                .WithExplicitExistingRules(existingRules)
                .Create();

            var resolvedRules = restController.GetAllRedirects();
            
            Assert.Equal(new [] { dto1, dto2 }, resolvedRules);
        }
        
/*        [Fact]
        public void Given_ExistingRedirects_Controller_ReturnsAllRedirects2()
        {
            var rulesCount = 10;

            var rule1 = DummyRule.Create();
            var rule2 = DummyRule.Create();
            var dto1 = new RedirectRuleDto();
            var dto2 = new RedirectRuleDto();
            
            var restController = RedirectRuleController()             
                .WithRules(new [] { rule1, rule2 })
                .WithMapper(r => r == rule1 ? dto1 : r == rule2 ? dto2 : Assert.False(true))
                .Create();

            var resolvedRules = restController.GetAllRedirects();
            
            Assert.Equal(new [] { dto1, dto2 }, resolvedRules);
        }*/
        
        [Fact]
        public void Given_ExistingRedirects_Controller_CreatesNewRedirect()
        {
            var restController = RedirectRuleController()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto("randomOldPath", "randomNewPath");
            var newRedirect = restController.Add(redirectDto);
            var expectedRedirect = restController.GetRedirect(newRedirect.Id.ExternalId);
            
            Assert.NotNull(expectedRedirect);
        }
        
        [Fact]
        public void Given_ExistingRedirects_Controller_UpdatesRedirect()
        {
            var rulesCount = 10;
            
            var restController = RedirectRuleController()
                .WithRandomExistingRules(rulesCount)
                .Create();
            
            var randomIndex = new Random().Next(rulesCount);
            var randomRedirectDto = restController
                .GetAllRedirects()
                .Skip(randomIndex)
                .FirstOrDefault();

            var expectedNewUrl = "/updatedNewUrl";
            randomRedirectDto.NewUrl = expectedNewUrl;
            
            restController.Update(randomRedirectDto);
            var updatedRedirect = restController.GetRedirect(randomRedirectDto.Id.ExternalId);
            
            Assert.Equal(expectedNewUrl, updatedRedirect?.NewUrl);
        }
        
        [Fact]
        public void Given_NotExistingRedirect_Controller_TriesToUpdateAndThrowsExceptionRedirectNotFound()
        {
            var restController = RedirectRuleController()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto(Guid.NewGuid(), "/NonExistentOldPath", "/randomNewUrl", RedirectType.Temporary);
                     
            Assert.Throws<KeyNotFoundException>(()=>restController.Update(redirectDto));
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Given_Redirects_Controller_ReturnsTrueIfFoundAndDeleted(bool doesExists)
        {
            var rulesCount = 10;
            var restController = RedirectRuleController()
                .WithRandomExistingRules(rulesCount)
                .Create();

            bool deleteResult;
            if (doesExists)
            {
                var randomIndex = new Random().Next(rulesCount);
                var randomRedirect = restController.GetAllRedirects().Skip(randomIndex).FirstOrDefault();
                deleteResult = restController.Delete(randomRedirect.Id.ExternalId);
            }
            else
            {
                deleteResult = restController.Delete(Guid.NewGuid());
            }
            
            Assert.Equal(doesExists, deleteResult);
        }
    }
}