using System;
using System.Collections.Generic;
using System.Linq;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Tests.Data
{
    public class RedirectRuleTestDataBuilder
    {
        private HashSet<RedirectRule> _redirectsHashSet = new HashSet<RedirectRule>();
        private readonly HashSet<Guid> _alreadyChangedRedirectRulesGuids = new HashSet<Guid>();

        public static RedirectRuleTestDataBuilder Start()
        {
            var testDataBuilder = new RedirectRuleTestDataBuilder();
            return testDataBuilder;
        }

        public void InitializeWithRandomData(int redirectRulesNumber)
        {
            for (var i = 0; i < redirectRulesNumber; i++)
            {
                var redirectRule = RandomDataGenerator.CreateRandomRedirectRule();
                _redirectsHashSet.Add(redirectRule);
            }
        }
        
        public void InitializeData(HashSet<RedirectRule> initializationData)
        {
            _redirectsHashSet = initializationData;
        }  
        
        public RedirectRule WithOldPath(string oldPath)
        {
            var redirectRule = ChangeData(r=>r.OldPattern = UrlPath.NormalizePath(oldPath));
            return redirectRule;
        }
        
        public RedirectRule WithOldPathAndNewUrl(string oldPath, string newUrl)
        {
            var redirectRule = ChangeData(r=>
            {
                r.OldPattern = UrlPath.NormalizePath(oldPath);
                r.NewPattern = newUrl;
                r.RedirectRuleType = RedirectRuleType.ExactMatch;
            });
            return redirectRule;
        }
        
        public RedirectRule WithOldPatternAndNewPattern(string oldPattern, string newPattern)
        {
            var redirectRule = ChangeData(r=>
            {
                r.OldPattern = oldPattern;
                r.NewPattern = newPattern;
                r.RedirectRuleType = RedirectRuleType.Regex;
            });
            return redirectRule;
        }

        private RedirectRule ChangeData(Action<RedirectRule> changeDataAction)
        {
            RedirectRule changedRedirectRule;

            while (true)
            {
                var randomRedirectRule = GetRandomRedirectRuleFromData();

                if (_alreadyChangedRedirectRulesGuids.Contains(randomRedirectRule.RuleId))
                {
                    continue;
                }

                changeDataAction.Invoke(randomRedirectRule);
                _alreadyChangedRedirectRulesGuids.Add(randomRedirectRule.RuleId);
                
                changedRedirectRule = randomRedirectRule;
                break;
            }

            return changedRedirectRule;
        }

        private RedirectRule GetRandomRedirectRuleFromData()
        {
            var randomIndex = new Random().Next(_redirectsHashSet.Count);
            return _redirectsHashSet.ElementAt(randomIndex);
        }

        public HashSet<RedirectRule> GetData()
        {
            return _redirectsHashSet;
        }
    }
}