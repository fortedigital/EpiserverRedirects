using Forte.EpiserverRedirects.Model;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Model
{
    public class UrlPathTests
    {
        [InlineData("/some/path/", "/some/path")]
        [InlineData("some/path/", "/some/path")]
        [InlineData("some/path", "/some/path")]
        [InlineData("", "")]
        [InlineData(null, "")]
        [Theory]
        public void GivenInputPath_WhenNormalize_ReturnsExpectedResult(string inputPath, string expectedResult)
        {
            var result = UrlPath.NormalizePath(inputPath);
            
            Assert.Equal(expectedResult, result);
        }
    }
}