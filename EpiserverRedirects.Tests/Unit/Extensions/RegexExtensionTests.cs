using System;
using Forte.EpiserverRedirects.Extensions;
using Forte.EpiserverRedirects.Tests.Data;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Unit.Extensions;

public class RegexExtensionTests
{
    [Fact]
    public void Given_AnyString_AppendStartAndEndOfStringTokenToThePattern()
    {
        const int maxValue = 50;
        var rnd = new Random();
        var maxStringLength = rnd.Next(maxValue); 
        var randomString = RandomDataGenerator.GetRandomString(rnd, maxStringLength);

        var result = randomString.ToStrictRegexPattern();
        
        Assert.StartsWith("^", result);
        Assert.EndsWith("$", result);
    }
}