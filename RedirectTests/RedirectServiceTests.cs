using System;
using System.Collections.Generic;
using System.Linq;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectType;
using RedirectTests.Data;
using Xunit;

namespace RedirectTests
{
    public class RedirectServiceTests
    {
        public static IEnumerable<object[]> NoExistingRedirectTestCase =>
            new []
            {
                new object[]
                {
                    "/oldPathThatDoesntHaveARedirection",
                    RedirectRuleTestDataBuilder.Start().GetData()
                }
            };
        [Theory]
        [MemberData(nameof(NoExistingRedirectTestCase))]
        public void Redirect_NoExistingRedirect_ReturnsNull(string oldPath, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var redirectRuleResolver = RedirectRuleResolver.Register().WithExistingRules(existingRedirects).Create();
            
            var redirect = redirectRuleResolver.Resolve().GetRedirectRule(oldPath);
            
            Assert.Null(redirect);
        }

        public static IEnumerable<object[]> ExistingRedirectTestCase =>
            new[]
            {
                new object[] { "/oldPath2", RedirectRuleTestDataBuilder.Start().WithOldPathAndNewUrl("/oldPath2", "/newUrl2").GetData(), "/newUrl2" }
            };
        [Theory]
        [MemberData(nameof(ExistingRedirectTestCase))]
        public void Redirect_ExistingRedirect_ReturnsNewPath(string oldPath, Dictionary<Guid, RedirectRule> existingRedirects, string expectedNewPath)
        {
            var redirectRuleResolver = RedirectRuleResolver.Register().WithExistingRules(existingRedirects).Create();
            
            var redirect = redirectRuleResolver.Resolve().GetRedirectRule(oldPath);
            
            Assert.Equal(expectedNewPath, redirect.NewUrl);
        }

        public static IEnumerable<object[]> ExistingRedirectsTestCase =>
            new[]
            {
                new object[] { 5, RedirectRuleTestDataBuilder.Start(5).GetData()}
            };
        [Theory]
        [MemberData(nameof(ExistingRedirectsTestCase))]
        public void Redirect_ExistingRedirects_ReturnsAllRedirects(int redirectsCount, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var redirectRuleResolver = RedirectRuleResolver.Register().WithExistingRules(existingRedirects).Create();

            var redirects = redirectRuleResolver.Resolve().GetAllRedirectRules();
            
            Assert.Equal(redirectsCount, redirects.Count());
        }
        
        public static IEnumerable<object[]> CreateRedirectTestCase =>
            new[]
            {
                new object[] {RedirectRuleTestDataBuilder.Start().GetData()}
            };
        [Theory]
        [MemberData(nameof(CreateRedirectTestCase))]
        public void Redirect_CreateRedirect_CreatesNewRedirect(Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var redirectRuleResolver = RedirectRuleResolver.Register().WithExistingRules(existingRedirects).Create();
            var redirectService = redirectRuleResolver.Resolve();

            var redirectDto = new RedirectRuleDto("randomOldPath", "randomNewPath");
            var newRedirect = redirectService.CreateRedirect(redirectDto);
            var allRedirects = redirectService.GetAllRedirectRules();
            
            Assert.True(allRedirects.Any(r=>r.Id == newRedirect.Id));
        }
        
        public static IEnumerable<object[]> UpdateRedirectTestCase =>
            new[]
            {
                new object[] { "updatedNewPath", RedirectRuleTestDataBuilder.Start().GetData()}
            };
        [Theory]
        [MemberData(nameof(UpdateRedirectTestCase))]
        public void Redirect_UpdateRedirect_UpdatesRedirect(string newUrl, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var redirectRuleResolver = RedirectRuleResolver.Register().WithExistingRules(existingRedirects).Create();
            var redirectService = redirectRuleResolver.Resolve();
            
            var randomIndex = new Random().Next(existingRedirects.Count);
            var randomRedirect = redirectService.GetAllRedirectRules().Skip(randomIndex).FirstOrDefault();

            var randomRedirectDto = RedirectRuleMapper.ModelToDto(randomRedirect);
            randomRedirectDto.NewUrl = newUrl;
            
            redirectService.UpdateRedirect(randomRedirectDto);
            var updatedRedirect = redirectService.GetRedirectRule(randomRedirectDto.OldPath);
            
            Assert.Equal(newUrl, updatedRedirect.NewUrl);
        }
        
        [Theory]
        [MemberData(nameof(UpdateRedirectTestCase))]
        public void Redirect_UpdateRedirect_ThrowsExceptionRedirectNotFound(string newPath, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var redirectRuleResolver = RedirectRuleResolver.Register().WithExistingRules(existingRedirects).Create();
            
            var redirectDto = new RedirectRuleDto(Guid.NewGuid(),"/oldPath", "newUrl", RedirectType.Temporary);
                     
            Assert.Throws<KeyNotFoundException>(()=>redirectRuleResolver.Resolve().UpdateRedirect(redirectDto));
        }
        
        public static IEnumerable<object[]> DeleteRedirectTestCase =>
            new[]
            {
                new object[] { true, RedirectRuleTestDataBuilder.Start().GetData()},
                new object[] { false, RedirectRuleTestDataBuilder.Start().GetData()},
            };
        [Theory]
        [MemberData(nameof(DeleteRedirectTestCase))]
        public void Redirect_DeleteRedirect_ReturnsTrueIfSuccessful(bool doesExists, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var redirectRuleResolver = RedirectRuleResolver.Register().WithExistingRules(existingRedirects).Create();
            var redirectService = redirectRuleResolver.Resolve();

            bool deleteResult;
            if (doesExists)
            {
                var randomIndex = new Random().Next(existingRedirects.Count);
                var randomRedirect = redirectService.GetAllRedirectRules().Skip(randomIndex).FirstOrDefault();
                deleteResult = redirectService.DeleteRedirect(randomRedirect.Id.ExternalId);
            }
            else
            {
                deleteResult = redirectService.DeleteRedirect(Guid.NewGuid());
            }
            
            Assert.Equal(doesExists, deleteResult);
        }
        
    }
}