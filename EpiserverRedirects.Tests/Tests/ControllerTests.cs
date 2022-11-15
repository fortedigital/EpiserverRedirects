using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Tests.Builder.WithRepository;
using Forte.EpiserverRedirects.Tests.Data;
using Forte.EpiserverRedirects.Tests.RestExtensions;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Tests
{
    public class ControllerTests
    {
        private static ControllerBuilder RedirectRuleController() => new ControllerBuilder();

        [Fact]
        public void Given_ExistingRedirects_Controller_ReturnsAllRedirects()
        {
            var rule1 = RandomDataGenerator.CreateRandomRedirectRule();
            var rule2 = RandomDataGenerator.CreateRandomRedirectRule();
            var existingRules = new HashSet<RedirectRuleModel>
            {
                rule1,
                rule2
            };
            var dto1 = new RedirectRuleDto();
            var dto2 = new RedirectRuleDto();
            
            var restController = RedirectRuleController()
                .WithExplicitExistingRules(existingRules)
                .WithMapper(r => r == rule1 ? dto1 : r == rule2 ? dto2 : null)
                .WithHttpResponseHeaders()
                .Create();

            var resolvedRules = restController
                .Get()
                .GetEntitiesFromActionResult();
            
            Assert.Equal(new[] { dto1, dto2 }, resolvedRules);
        }

        [Fact]
        public void Given_ExistingRedirects_Controller_CreatesNewRedirect()
        {
            var restController = RedirectRuleController()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto
            {
                OldPattern = "randomOldPath",
                NewPattern = "randomNewPath"
            };
            var newRedirect = restController.Post(redirectDto).GetEntityFromActionResult();
            var expectedRedirect = restController.Get(newRedirect.Id.Value).GetEntityFromActionResult();

            Assert.NotNull(expectedRedirect);
        }

        [Fact]
        public void Given_ExistingRedirects_Controller_UpdatesRedirect()
        {
            var rulesCount = 10;

            var restController = RedirectRuleController()
                .WithRandomExistingRules(rulesCount)
                .WithHttpResponseHeaders()
                .Create();

            var randomIndex = new Random().Next(rulesCount);
            var randomRedirectDto = restController
                .Get()
                .GetEntitiesFromActionResult()
                .Skip(randomIndex)
                .FirstOrDefault();

            var expectedNewUrl = "/updatedNewUrl";
            randomRedirectDto.NewPattern = expectedNewUrl;

            restController.Put(randomRedirectDto);
            var updatedRedirect = restController
                .Get(randomRedirectDto.Id.Value)
                .GetEntityFromActionResult();

            Assert.Equal(expectedNewUrl, updatedRedirect?.NewPattern);
        }

        [Fact]
        public void Given_NotExistingRedirect_Controller_TriesToUpdateAndThrowsExceptionRedirectNotFound()
        {
            var restController = RedirectRuleController()
                .WithRandomExistingRules()
                .Create();

            var redirectDto = new RedirectRuleDto
            {
                Id = Guid.NewGuid(),
                OldPattern = "/NonExistentOldPath",
                NewPattern = "/randomNewUrl",
                RedirectType = RedirectType.Temporary
            };
            Assert.Throws<KeyNotFoundException>(() => restController.Put(redirectDto));
        }

        [Theory]
        [InlineData(true, HttpStatusCode.OK)]
        [InlineData(false, HttpStatusCode.Conflict)]
        public void Given_Redirects_Controller_ReturnsTrueIfFoundAndDeleted(bool doesExists, HttpStatusCode result)
        {
            var rulesCount = 10;
            var restController = RedirectRuleController()
                .WithHttpResponseHeaders()
                .WithRandomExistingRules(rulesCount)
                .Create();

            HttpStatusCode deleteResult;
            if (doesExists)
            {
                var randomIndex = new Random().Next(rulesCount);
                var randomRedirect = restController
                    .Get()
                    .GetEntitiesFromActionResult()
                    .Skip(randomIndex)
                    .FirstOrDefault();
                deleteResult = restController
                    .Delete(randomRedirect.Id.Value)
                    .GetStatusCodeFromActionResult();
            }
            else
            {
                deleteResult = restController
                    .Delete(Guid.NewGuid())
                    .GetStatusCodeFromActionResult();
            }

            Assert.Equal(result, deleteResult);
        }
    }
}