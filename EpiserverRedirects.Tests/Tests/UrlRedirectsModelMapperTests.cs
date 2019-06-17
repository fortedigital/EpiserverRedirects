using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.UrlRewritePlugin;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Tests
{
    public class UrlRedirectsModelMapperTests
    {
        [Theory]
        [InlineData("/test/redir/", "/test/redir")]
        [InlineData("/test/", "/test")]
        [InlineData("/test/redir/test/test/test///", "/test/redir/test/test/test")]
        [InlineData("////test/redir/test/test/test/", "/test/redir/test/test/test")]
        [InlineData("////test/redir/test/test/test///", "/test/redir/test/test/test")]
        public void Given_OldUrlWithSlashesAtEndOrStart_Should_MapWithTrimmedEndSlashAndSingleStartSlash(string oldUrl, string expected)
        {
            var dto = new UrlRedirectsDto(oldUrl, 0, UrlRedirectsType.Manual, 10,
                RedirectStatusCode.Temporary);

            var mappedModel = dto.MapToUrlRewriteModel();

            Assert.Equal(expected, mappedModel.OldUrl);
        }

        [Theory]
        [InlineData("/test/redir")]
        [InlineData("/test")]
        [InlineData("/en/test/redir")]
        [InlineData("/test/redir")]
        [InlineData("/test/redir")]
        public void Given_OldUrlWithoutSlashAtEnd_Should_MapWithSameOldUrl(string oldUrl)
        {
            var dto = new UrlRedirectsDto(oldUrl, 0, UrlRedirectsType.Manual, 10,
                RedirectStatusCode.Temporary);

            var mappedModel = dto.MapToUrlRewriteModel();

            Assert.Equal(oldUrl, mappedModel.OldUrl);
        }
    }
}
