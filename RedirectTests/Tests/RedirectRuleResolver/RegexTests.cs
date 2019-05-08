using System;
using RedirectTests.Tests.Builder.Resolver;
using Xunit;

namespace RedirectTests.Tests.RedirectRuleResolver
{
    public class RegexTests
    {
        private static RegexResolverBuilder RegexResolver() => new RegexResolverBuilder();

        [Fact]
        public async void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = RegexResolver()
                .Create();

            throw new NotImplementedException();
        }
    }
}