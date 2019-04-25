using System;
using System.Collections.Generic;
using System.Linq;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Model.UrlPath;

namespace RedirectTests.Data
{
    public class RedirectRuleTestDataBuilder
    {
        private Dictionary<Guid, RedirectRule> _redirectsRuleDataDictionary = new Dictionary<Guid, RedirectRule>();
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
                _redirectsRuleDataDictionary.Add(redirectRule.Id.ExternalId, redirectRule);
            }
        }
        
        public void InitializeData(Dictionary<Guid, RedirectRule> initializationData)
        {
            _redirectsRuleDataDictionary = initializationData;
        }  
        
        public RedirectRule WithOldPath(string oldPath)
        {
            var redirectRule = ChangeData(r=>r.OldPath = UrlPath.Parse(oldPath));
            return redirectRule;
        }
        
        public RedirectRule WithOldPathAndNewUrl(string oldPath, string newUrl)
        {
            var redirectRule = ChangeData(r=>
            {
                r.OldPath = UrlPath.Parse(oldPath);
                r.NewUrl = newUrl;
            });
            return redirectRule;
        }

        private RedirectRule ChangeData(Action<RedirectRule> changeDataAction)
        {
            RedirectRule changedRedirectRule;


            while (true)
            {
                var randomRedirectRule = GetRandomRedirectRuleFromData();

                if (_alreadyChangedRedirectRulesGuids.Contains(randomRedirectRule.Id.ExternalId))
                    continue;

                changeDataAction.Invoke(randomRedirectRule);
                _alreadyChangedRedirectRulesGuids.Add(randomRedirectRule.Id.ExternalId);
                
                changedRedirectRule = randomRedirectRule;
                break;
            }

            return changedRedirectRule;
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