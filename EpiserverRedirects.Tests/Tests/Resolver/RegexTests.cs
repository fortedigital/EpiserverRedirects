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
    public class RegexTests
    {
        private static RegexResolverBuilder RegexResolver() => new RegexResolverBuilder();

        [Fact]
        public async void Given_NoExistingRules_Resolve_ReturnsNull()
        {
            var resolver = RegexResolver()
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }

        [Fact]
        public async void Given_NonMatchingRules_Resolve_ReturnsNull()
        {
            var resolver = RegexResolver()
                .WithRandomExistingRules(10)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/dummyPath"));
            
            Assert.Null(redirect?.Id);
        }
        
        
        [Fact]
        public async void Given_MatchingRule_Resolve_ReturnsTheRule()
        {
            var resolver = RegexResolver()
                .WithRandomExistingRules()
                .WithRule(r=>r.WithOldPatternAndNewPattern("/oldPattern", "newPattern/$1"), out var expectedRule)
                .Create();
            
            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/oldPattern"));

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
            var resolver = RegexResolver()
                .WithExplicitExistingRules(existingRules)
                .WithRule(r => r.WithOldPatternAndNewPattern("oldPattern", "newPattern/$1"), out var expectedRule)
                .Create();

            var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/oldPattern"));
            Assert.Null(redirect?.Id);
        }
        
        [Fact]
         public async void Given_MatchingRuleByHost_Resolve_ReturnsRule()
         {
             var rule1 = RandomDataGenerator.CreateRandomRedirectRule();
             var rule2 = RandomDataGenerator.CreateRandomRedirectRule();

             var guid = Guid.NewGuid();
             SiteDefinition.Current.Id = guid;
             
             rule1.HostId = guid;
             rule1.OldPattern = "/oldPattern1";
             rule1.NewPattern = "newPattern1/$1";
             rule1.RedirectRuleType = RedirectRuleType.Regex; 
             rule2.HostId = Guid.NewGuid();
             rule2.OldPattern = "/oldPattern2";
             rule2.NewPattern = "newPattern2/$1";
             rule2.RedirectRuleType = RedirectRuleType.Regex; 
             
             var existingRules = new HashSet<RedirectRuleModel>
             {
                 rule1,
                 rule2
             };
             var resolver = RegexResolver()
                 .WithExplicitExistingRules(existingRules)
                 .Create();
        
             var redirect = await resolver.ResolveRedirectRuleAsync(UrlPath.Parse("/oldPattern1"));
             Assert.Equal(rule1.RuleId, redirect.Id);
         }
    }
}