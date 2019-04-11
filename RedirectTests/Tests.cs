using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data;
using Xunit;
using Redirects;
using Redirects.Repository;

namespace RedirectTests
{
    public class Tests
    {
        private static Guid GuidForTestData;
        private static int GuidAccessCount = 0;
        private static Guid GetSameGuidTwice()
        {
            if (GuidAccessCount%2 == 0)
                GuidForTestData = Guid.NewGuid();

            GuidAccessCount++;
            return GuidForTestData;
        }
        private static readonly Dictionary<Guid, RedirectModel> RedirectsData = new Dictionary<Guid, RedirectModel>
        {  
            {GetSameGuidTwice(), new RedirectModel(GetSameGuidTwice(), "/oldPath1", "/newPath1")},
            {GetSameGuidTwice(), new RedirectModel(GetSameGuidTwice(), "/oldPath2", "/newPath2")},
            {GetSameGuidTwice(), new RedirectModel(GetSameGuidTwice(), "/oldPath3", "/newPath3")},
            {GetSameGuidTwice(), new RedirectModel(GetSameGuidTwice(), "/oldPath4", "/newPath4")},
            {GetSameGuidTwice(), new RedirectModel(GetSameGuidTwice(), "/oldPath5", "/newPath1")},
        };
        

        public static IEnumerable<object[]> NoExistingRedirectTestCase =>
            new []
            {
                new object[] {"/oldPathThatDoesntHaveARedirection", RedirectsData }
            };
        [Theory]
        [MemberData(nameof(NoExistingRedirectTestCase))]
        public void Redirect_NoExistingRedirect_ReturnsNull(string oldPath, Dictionary<Guid, RedirectModel> existingRedirects)
        {
            var repository = new TestRepository(existingRedirects);
            var redirectService = new RedirectService(repository);
            
            var redirect = redirectService.GetRedirect(oldPath);
            
            Assert.Null(redirect);
        }

        public static IEnumerable<object[]> ExistingRedirectTestCase =>
            new[]
            {
                new object[] { "/oldPath2", RedirectsData, "/newPath2" }
            };
        [Theory]
        [MemberData(nameof(ExistingRedirectTestCase))]
        public void Redirect_ExistingRedirect_ReturnsNewPath(string oldPath, Dictionary<Guid, RedirectModel> existingRedirects, string expectedNewPath)
        {
            var repository = new TestRepository(existingRedirects);
            var redirectService = new RedirectService(repository);
            
            var redirect = redirectService.GetRedirect(oldPath);
            
            Assert.Equal(expectedNewPath, redirect.NewPath);
        }

        public static IEnumerable<object[]> ExistingRedirectsTestCase =>
            new[]
            {
                new object[] { 5, RedirectsData}
            };
        [Theory]
        [MemberData(nameof(ExistingRedirectsTestCase))]
        public void Redirect_ExistingRedirects_ReturnsAllRedirects(int redirectsCount, Dictionary<Guid, RedirectModel> existingRedirects)
        {
            var repository = new TestRepository(existingRedirects);
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
        public void Redirect_CreateRedirect_CreatesNewRedirect(Dictionary<Guid, RedirectModel> existingRedirects)
        {
            var repository = new TestRepository(existingRedirects);
            var redirectService = new RedirectService(repository);

            var redirectVM = new RedirectModel("randomOldPath", "randomNewPath");
            var newRedirect = redirectService.CreateRedirect(redirectVM);
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
        public void Redirect_UpdateRedirect_UpdatesRedirect(string newPath, Dictionary<Guid, RedirectModel> existingRedirects)
        {
            var repository = new TestRepository(existingRedirects);
            var redirectService = new RedirectService(repository);

            var randomIndex = new Random().Next(existingRedirects.Count);
            var randomRedirect = redirectService.GetAllRedirects().Skip(randomIndex).FirstOrDefault();
            
            var redirectVM = new RedirectModel(randomRedirect.Id, randomRedirect.OldPath, newPath);
            redirectService.UpdateRedirect(redirectVM);
            var updatedRedirect = redirectService.GetRedirect(randomRedirect.OldPath);
            
            Assert.Equal(newPath, updatedRedirect.NewPath);
        }
        
        [Theory]
        [MemberData(nameof(UpdateRedirectTestCase))]
        public void Redirect_UpdateRedirect_ThrowsExceptionRedirectNotFound(string newPath, Dictionary<Guid, RedirectModel> existingRedirects)
        {
            var repository = new TestRepository(existingRedirects);
            var redirectService = new RedirectService(repository);
            
            var redirectVM = new RedirectModel(Guid.NewGuid());
                     
            Assert.Throws<KeyNotFoundException>(()=>redirectService.UpdateRedirect(redirectVM));
        }
        
        public static IEnumerable<object[]> DeleteRedirectTestCase =>
            new[]
            {
                new object[] { true, RedirectsData},
                new object[] { false, RedirectsData},
            };
        [Theory]
        [MemberData(nameof(DeleteRedirectTestCase))]
        public void Redirect_DeleteRedirect_ReturnsTrueIfSuccessful(bool doesExists, Dictionary<Guid, RedirectModel> existingRedirects)
        {
            var repository = new TestRepository(existingRedirects);
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