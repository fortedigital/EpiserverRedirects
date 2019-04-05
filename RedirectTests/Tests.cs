using System.Collections.Generic;
using Xunit;
using Redirects;

namespace RedirectTests
{
    public class Tests
    {
        private static readonly Dictionary<string, string> RedirectsData = new Dictionary<string, string>
        {
            {"/oldPath1", "/newPath1"},
            {"/oldPath2", "/newPath2"},
            {"/oldPath3", "/newPath3"},
            {"/oldPath4", "/newPath4"},
            {"/oldPath5", "/newPath1"},
        };

        private static IEnumerable<object[]> NoExistingRedirectsTestCase =>
            new []
            {
                new object[] {"/oldPathThatDoesntHaveARedirection", RedirectsData }
            };

        [Theory]
        [MemberData(nameof(NoExistingRedirectsTestCase))]
        public void Redirect_NoExistingRedirect_ReturnsNull(string oldPath, Dictionary<string, string> existingRedirects)
        {
            var repository = new DictionaryRepository(existingRedirects);
            var redirectService = new RedirectService(repository);
            
            var newPath = redirectService.FindRedirect(oldPath);
            
            Assert.Null(newPath);
        }

        private static IEnumerable<object[]> ExistingRedirectsTestCase =>
            new[]
            {
                new object[] { "/oldPath2", RedirectsData, "/newPath2" }
            };

        [Theory]
        [MemberData(nameof(ExistingRedirectsTestCase))]
        public void Redirect_ExistingRedirect_ReturnsNewPath(string oldPath, Dictionary<string, string> existingRedirects, string expectedNewPath)
        {
            var repository = new DictionaryRepository(existingRedirects);
            var redirectService = new RedirectService(repository);
            
            var newPath = redirectService.FindRedirect(oldPath);
            
            Assert.Equal(expectedNewPath, newPath);
        }
    }
}