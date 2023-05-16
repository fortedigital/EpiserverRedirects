using System;
using System.Collections.Generic;
using EPiServer.Web;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Tests.Builder.WithRepository.Resolver;
using Forte.EpiserverRedirects.Tests.Data;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Tests.Resolver
{
    public class ExactMatchTests
    {
        private static ExactMatchResolverBuilder RedirectRuleResolver() => new ExactMatchResolverBuilder();

        [Fact]
        public async void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }

        [Fact]
        public async void Given_NonMatchingRules_Resolve_ReturnsNull()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }
        
        [Fact]
        public async void Given_MatchingRule_Resolve_ReturnsTheRule()
        {
            var resolver = RedirectRuleResolver()
                .WithRandomExistingRules(10)
                .WithRule(r => r.WithOldPath("/dummyPath"), out var expectedRule)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse(expectedRule.OldPattern));
            
            Assert.Equal(expectedRule.RuleId, redirect?.Id);
        }

        [Fact]
        public async void Given_NonMatchingRulesByHost_Resolve_ReturnsNull()
        {
            var rule1 = RandomDataGenerator.CreateRandomRedirectRule();
            var rule2 = RandomDataGenerator.CreateRandomRedirectRule();
            var rule3 = RandomDataGenerator.CreateRandomRedirectRule();
            rule1.HostId = Guid.NewGuid();
            rule2.HostId = Guid.NewGuid();
            rule3.HostId = Guid.NewGuid();
            var existingRules = new HashSet<RedirectRuleModel>
            {
                rule1,
                rule2,
                rule3
            };
            var resolver = RedirectRuleResolver()
                .WithExplicitExistingRules(existingRules)
                .WithRule(r => r.WithOldPath("/dummyPath"), out var expectedRule)
                .Create();

            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse(expectedRule.OldPattern));
            Assert.Null(redirect?.Id);
        }
        
        [Fact]
        public async void Given_MatchingRuleByHost_Resolve_ReturnsRule()
        {
            var rule1 = RandomDataGenerator.CreateRandomRedirectRule();
            var rule2 = RandomDataGenerator.CreateRandomRedirectRule();
            var rule3 = RandomDataGenerator.CreateRandomRedirectRule();
            
            var guid = Guid.NewGuid();
            SiteDefinition.Current.Id = guid;
            
            rule1.HostId = guid;
            rule1.OldPattern = "/dummyPath";
            rule2.HostId = Guid.NewGuid();
            rule3.HostId = Guid.NewGuid();
            var existingRules = new HashSet<RedirectRuleModel>
            {
                rule1,
                rule2,
                rule3
            };
            var resolver = RedirectRuleResolver()
                .WithExplicitExistingRules(existingRules)
                .Create();

            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse(rule1.OldPattern));
            Assert.Equal(rule1.RuleId, redirect.Id);
        }
    }
}