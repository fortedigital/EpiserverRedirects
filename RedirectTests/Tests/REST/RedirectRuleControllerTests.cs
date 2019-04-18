using System;
using System.Collections.Generic;
using System.Linq;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectType;
using Xunit;

namespace RedirectTests.Tests.REST
{
    public class RedirectRuleControllerTests
    {
        private static RedirectRuleControllerBuilder RedirectRuleController() => new RedirectRuleControllerBuilder();
        
        [Fact]
        public void Given_ExistingRedirects_Controller_ReturnsAllRedirects()
        {
            var rulesCount = 10;

            var restController = RedirectRuleController()
                .WithTestMapping()
                .WithRandomExistingRules(rulesCount)
                .Create();

            var resolvedRules = restController.GetAllRedirects();
            
            Assert.Equal(rulesCount, resolvedRules.Count());
        }
        
        [Fact]
        public void Given_ExistingRedirects_Controller_CreatesNewRedirect()
        {
            var restController = RedirectRuleController()
                .WithTestMapping()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto("randomOldPath", "randomNewPath");
            var newRedirect = restController.CreateRedirect(redirectDto);
            var expectedRedirect = restController.GetRedirect(newRedirect.Id.ExternalId);
            
            Assert.NotNull(expectedRedirect);
        }
        
        [Fact]
        public void Given_ExistingRedirects_Controller_UpdatesRedirect()
        {
            var rulesCount = 10;
            
            var restController = RedirectRuleController()
                .WithTestMapping()
                .WithRandomExistingRules(rulesCount)
                .Create();
            
            var randomIndex = new Random().Next(rulesCount);
            var randomRedirectDto = restController
                .GetAllRedirects()
                .Skip(randomIndex)
                .FirstOrDefault();

            var expectedNewUrl = "/updatedNewUrl";
            randomRedirectDto.NewUrl = expectedNewUrl;
            
            restController.UpdateRedirect(randomRedirectDto);
            var updatedRedirect = restController.GetRedirect(randomRedirectDto.Id.ExternalId);
            
            Assert.Equal(expectedNewUrl, updatedRedirect?.NewUrl);
        }
        
        [Fact]
        public void Given_NotExistingRedirect_Controller_TriesToUpdateAndThrowsExceptionRedirectNotFound()
        {
            var restController = RedirectRuleController()
                .WithTestMapping()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto(Guid.NewGuid(), "/NonExistentOldPath", "/randomNewUrl", RedirectType.Temporary);
                     
            Assert.Throws<KeyNotFoundException>(()=>restController.UpdateRedirect(redirectDto));
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Given_Redirects_Controller_ReturnsTrueIfFoundAndDeleted(bool doesExists)
        {
            var rulesCount = 10;
            var restController = RedirectRuleController()
                .WithTestMapping()
                .WithRandomExistingRules(rulesCount)
                .Create();

            bool deleteResult;
            if (doesExists)
            {
                var randomIndex = new Random().Next(rulesCount);
                var randomRedirect = restController.GetAllRedirects().Skip(randomIndex).FirstOrDefault();
                deleteResult = restController.DeleteRedirect(randomRedirect.Id.ExternalId);
            }
            else
            {
                deleteResult = restController.DeleteRedirect(Guid.NewGuid());
            }
            
            Assert.Equal(doesExists, deleteResult);
        }
    }
}