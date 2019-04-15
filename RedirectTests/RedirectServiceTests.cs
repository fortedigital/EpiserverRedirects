using System;
using System.Collections.Generic;
using System.Linq;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Repository;
using Forte.RedirectMiddleware.Service;
using Xunit;

namespace RedirectTests
{
    public class RedirectServiceTests
    {
        private static Guid _guidForTestData;
        private static int _guidAccessCount = 0;
        private static Guid GetSameGuidTwice()
        {
            if (_guidAccessCount%2 == 0)
                _guidForTestData = Guid.NewGuid();

            _guidAccessCount++;
            return _guidForTestData;
        }
        private static readonly Dictionary<Guid, RedirectRule> RedirectsData = new Dictionary<Guid, RedirectRule>
        {  
            {GetSameGuidTwice(), new RedirectRule(GetSameGuidTwice(), "/oldPath1", "/newUrl", RedirectType.Temporary)},
            {GetSameGuidTwice(), new RedirectRule(GetSameGuidTwice(), "/oldPath2", "/newUrl2", RedirectType.Temporary)},
            {GetSameGuidTwice(), new RedirectRule(GetSameGuidTwice(), "/oldPath3", "/newUrl3", RedirectType.Temporary)},
            {GetSameGuidTwice(), new RedirectRule(GetSameGuidTwice(), "/oldPath4", "/newUrl4", RedirectType.Temporary)},
            {GetSameGuidTwice(), new RedirectRule(GetSameGuidTwice(), "/oldPath5", "/newUrl1", RedirectType.Temporary)},
        };
        

        public static IEnumerable<object[]> NoExistingRedirectTestCase =>
            new []
            {
                new object[] {"/oldPathThatDoesntHaveARedirection", RedirectsData }
            };
        [Theory]
        [MemberData(nameof(NoExistingRedirectTestCase))]
        public void Redirect_NoExistingRedirect_ReturnsNull(string oldPath, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var repository = new TestRedirectRuleRepository(existingRedirects);
            var redirectService = new RedirectService(repository);
            
            var redirect = redirectService.GetRedirect(oldPath);
            
            Assert.Null(redirect);
        }

        public static IEnumerable<object[]> ExistingRedirectTestCase =>
            new[]
            {
                new object[] { "/oldPath2", RedirectsData, "/newUrl2" }
            };
        [Theory]
        [MemberData(nameof(ExistingRedirectTestCase))]
        public void Redirect_ExistingRedirect_ReturnsNewPath(string oldPath, Dictionary<Guid, RedirectRule> existingRedirects, string expectedNewPath)
        {
            var repository = new TestRedirectRuleRepository(existingRedirects);
            var redirectService = new RedirectService(repository);
            
            var redirect = redirectService.GetRedirect(oldPath);
            
            Assert.Equal(expectedNewPath, redirect.NewUrl);
        }

        public static IEnumerable<object[]> ExistingRedirectsTestCase =>
            new[]
            {
                new object[] { 5, RedirectsData}
            };
        [Theory]
        [MemberData(nameof(ExistingRedirectsTestCase))]
        public void Redirect_ExistingRedirects_ReturnsAllRedirects(int redirectsCount, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var repository = new TestRedirectRuleRepository(existingRedirects);
            var redirectService = new RedirectService(repository);

            var redirects = redirectService.GetAllRedirects();
            
            Assert.Equal(redirectsCount, redirects.Count());
        }
        
        public static IEnumerable<object[]> CreateRedirectTestCase =>
            new[]
            {
                new object[] {RedirectsData}
            };
        [Theory]
        [MemberData(nameof(CreateRedirectTestCase))]
        public void Redirect_CreateRedirect_CreatesNewRedirect(Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var repository = new TestRedirectRuleRepository(existingRedirects);
            var redirectService = new RedirectService(repository);

            var redirectDto = new RedirectRuleDto("randomOldPath", "randomNewPath");
            var newRedirect = redirectService.CreateRedirect(redirectDto);
            var allRedirects = redirectService.GetAllRedirects();
            
            Assert.True(allRedirects.Any(r=>r.Id == newRedirect.Id));
        }
        
        public static IEnumerable<object[]> UpdateRedirectTestCase =>
            new[]
            {
                new object[] { "updatedNewPath", RedirectsData}
            };
        [Theory]
        [MemberData(nameof(UpdateRedirectTestCase))]
        public void Redirect_UpdateRedirect_UpdatesRedirect(string newUrl, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var repository = new TestRedirectRuleRepository(existingRedirects);
            var redirectService = new RedirectService(repository);

            var randomIndex = new Random().Next(existingRedirects.Count);
            var randomRedirectDto = redirectService.GetAllRedirects().Skip(randomIndex).FirstOrDefault();
            randomRedirectDto.NewUrl = newUrl;
            redirectService.UpdateRedirect(randomRedirectDto);
            var updatedRedirect = redirectService.GetRedirect(randomRedirectDto.OldPath);
            
            Assert.Equal(newUrl, updatedRedirect.NewUrl);
        }
        
        [Theory]
        [MemberData(nameof(UpdateRedirectTestCase))]
        public void Redirect_UpdateRedirect_ThrowsExceptionRedirectNotFound(string newPath, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var repository = new TestRedirectRuleRepository(existingRedirects);
            var redirectService = new RedirectService(repository);
            
            var redirectDto = new RedirectRuleDto(Guid.NewGuid(),"/oldPath", "newUrl", RedirectType.Temporary);
                     
            Assert.Throws<KeyNotFoundException>(()=>redirectService.UpdateRedirect(redirectDto));
        }
        
        public static IEnumerable<object[]> DeleteRedirectTestCase =>
            new[]
            {
                new object[] { true, RedirectsData},
                new object[] { false, RedirectsData},
            };
        [Theory]
        [MemberData(nameof(DeleteRedirectTestCase))]
        public void Redirect_DeleteRedirect_ReturnsTrueIfSuccessful(bool doesExists, Dictionary<Guid, RedirectRule> existingRedirects)
        {
            var repository = new TestRedirectRuleRepository(existingRedirects);
            var redirectService = new RedirectService(repository);

            bool deleteResult;
            if (doesExists)
            {
                var randomIndex = new Random().Next(existingRedirects.Count);
                var randomRedirect = redirectService.GetAllRedirects().Skip(randomIndex).FirstOrDefault();
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