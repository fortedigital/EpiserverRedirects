using System;
using Forte.EpiserverRedirects.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace Forte.EpiserverRedirects.Tests.Builder;

public static class ContentProvidersOptionsBuilder
{
    public static Mock<IOptions<ContentProvidersOptions>> Create()
    {
        var options = new ContentProvidersOptions
        {
            ContentProviders = new[]
            {
                new ContentProviderOption(new Guid("6A03B87C-F1BE-4B02-9514-A2206498C817"), null, "Test null"),
                new ContentProviderOption(new Guid("B941C44D-0F06-4EF8-B0F1-A8073DCA31B5"), "TestProvider",
                    "Test provider"),
            }
        };

        var mock = new Mock<IOptions<ContentProvidersOptions>>();

        mock.Setup(x => x.Value).Returns(options);

        return mock;
    }
}