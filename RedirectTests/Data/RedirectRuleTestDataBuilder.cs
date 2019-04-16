using System;
using System.Collections.Generic;
using System.Linq;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectType;

namespace RedirectTests.Data
{
    public class RedirectRuleTestDataBuilder
    {
        private const int DefaultRedirectRulesNumber = 5;
        private readonly Dictionary<Guid, RedirectRule> _redirectsRuleDataDictionary = new Dictionary<Guid, RedirectRule>();

        public static RedirectRuleTestDataBuilder Start(int redirectRulesNumber = DefaultRedirectRulesNumber)
        {
            var objectMother = new RedirectRuleTestDataBuilder();
            objectMother.InitializeData(redirectRulesNumber);
            return objectMother;
        }

        private void InitializeData(int redirectRulesNumber)
        {
            for (var i = 0; i < redirectRulesNumber; i++)
            {
                var redirectRule = RandomDataGenerator.CreateRandomRedirectRule();
                _redirectsRuleDataDictionary.Add(redirectRule.Id.ExternalId, redirectRule);
            }
        }     
        
        public RedirectRuleTestDataBuilder WithOldPath(string oldPath, int numberOfRedirectRulesToChange = 1)
        {
            ChangeData(r=>r.OldPath = UrlPath.Create(oldPath), numberOfRedirectRulesToChange);
            return this;
        }
        
        public RedirectRuleTestDataBuilder WithOldPathAndNewUrl(string oldPath, string newUrl, int numberOfRedirectRulesToChange = 1)
        {
            ChangeData(r=>
            {
                r.OldPath = UrlPath.Create(oldPath);
                r.NewUrl = newUrl;
            }, numberOfRedirectRulesToChange);
            return this;
        }      
        
        private void ChangeData(Action<RedirectRule> changeDataAction, int numberOfRedirectRulesToChange)
        {
            var alreadyChangedRedirectRulesGuids = new HashSet<Guid>();

            while (alreadyChangedRedirectRulesGuids.Count < numberOfRedirectRulesToChange)
            {
                var randomRedirectRule = GetRandomRedirectRuleFromData();

                if (alreadyChangedRedirectRulesGuids.Contains(randomRedirectRule.Id.ExternalId))
                    continue;

                changeDataAction.Invoke(randomRedirectRule);
                alreadyChangedRedirectRulesGuids.Add(randomRedirectRule.Id.ExternalId);
            }
        }

        private RedirectRule GetRandomRedirectRuleFromData()
        {
            var randomIndex = new Random().Next(_redirectsRuleDataDictionary.Count);
            return _redirectsRuleDataDictionary.ElementAt(randomIndex).Value;
        }

        public Dictionary<Guid, RedirectRule> GetData()
        {
            return _redirectsRuleDataDictionary;
        }

    }
}