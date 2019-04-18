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
        private static RedirectRuleControllerBuilder RedirectRuleController()
        {
            return new RedirectRuleControllerBuilder();
        }
        
        [Fact]
        public void Redirect_ExistingRedirects_ReturnsAllRedirects()
        {
            var rulesCount = 10;

            var restController = RedirectRuleController()
                .WithRandomExistingRules(rulesCount)
                .Create();

            var resolvedRules = restController.GetAllRedirectRules();
            
            Assert.Equal(rulesCount, resolvedRules.Count());
        }
        
        [Fact]
        public void Redirect_CreateRedirect_CreatesNewRedirect()
        {
            var restController = RedirectRuleController()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto("randomOldPath", "randomNewPath");
            var newRedirect = restController.CreateRedirect(redirectDto);
            var allRedirects = restController.GetAllRedirectRules();
            
            Assert.True(allRedirects.Any(r=>r.Id == newRedirect.Id));
        }
        
        [Fact]
        public void Redirect_UpdateRedirect_UpdatesRedirect()
        {
            var rulesCount = 10;
            
            var restController = RedirectRuleController()
                .WithRandomExistingRules(rulesCount)
                .Create();
            
            var randomIndex = new Random().Next(rulesCount);
            var randomRedirect = restController
                .GetAllRedirectRules()
                .Skip(randomIndex)
                .FirstOrDefault();

            var expectedNewUrl = "/updatedNewUrl";
            var randomRedirectDto = RedirectRuleMapper.ModelToDto(randomRedirect);
            randomRedirectDto.NewUrl = expectedNewUrl;
            
            restController.UpdateRedirect(randomRedirectDto);
            var updatedRedirect = restController.GetAllRedirectRules().FirstOrDefault(r=>r.Id == randomRedirectDto.Id);
            
            Assert.Equal(expectedNewUrl, updatedRedirect?.NewUrl);
        }
        
        [Fact]
        public void Redirect_UpdateRedirect_ThrowsExceptionRedirectNotFound()
        {
            var restController = RedirectRuleController()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto(Guid.NewGuid(), "/NonExistentOldPath", "/randomNewUrl", RedirectType.Temporary);
                     
            Assert.Throws<KeyNotFoundException>(()=>restController.UpdateRedirect(redirectDto));
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Redirect_DeleteRedirect_ReturnsTrueIfSuccessful(bool doesExists)
        {
            var rulesCount = 10;
            var restController = RedirectRuleController()
                .WithRandomExistingRules(rulesCount)
                .Create();

            bool deleteResult;
            if (doesExists)
            {
                var randomIndex = new Random().Next(rulesCount);
                var randomRedirect = restController.GetAllRedirectRules().Skip(randomIndex).FirstOrDefault();
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