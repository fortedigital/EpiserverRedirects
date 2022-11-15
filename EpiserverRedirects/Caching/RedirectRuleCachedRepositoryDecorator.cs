using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;


namespace Forte.EpiserverRedirects.Caching
{
    /// <summary>
    /// Represents cached repository access class
    /// This class does not cache GetAll() and Query() requests - as they are used in Redirects management UI
    /// This class caches only match-finding logic
    /// </summary>
    public class RedirectRuleCachedRepositoryDecorator : IRedirectRuleRepository
    {
        private static readonly object Locker = new object();
        public const string CacheKeyExact = "Forte.Episerver.RedirectRules.Exact";
        public const string CacheKeyRegex = "Forte.Episerver.RedirectRules.Regex";
        private readonly ICache _cache;
        private readonly IRedirectRuleRepository _repository;

        public RedirectRuleCachedRepositoryDecorator(IRedirectRuleRepository redirectRuleRepository, ICache cache)
        {
            _cache = cache;
            _repository = redirectRuleRepository;
        }

        public RedirectRuleModel GetById(Guid id) => _repository.GetById(id);

        public IList<RedirectRuleModel> GetAll() => _repository.GetAll();

        public IList<RedirectRuleModel> Query(out int total, RedirectRuleQuery query) => _repository.Query(out total, query);

        public IList<RedirectRuleModel> GetByContent(IList<int> contentIds) => _repository.GetByContent(contentIds);

        public RedirectRuleModel FindRegexMatch(string patern)
        {
            if (patern == null)
            {
                return null;
            }

            _cache.TryGet<Dictionary<string, RedirectRuleModel>>(CacheKeyRegex, out var ruleSet);
            if (ruleSet == null)
            {
                ruleSet = new Dictionary<string, RedirectRuleModel>();
                _cache.Add(CacheKeyRegex, ruleSet);
            }

            if (ruleSet.TryGetValue(patern, out var rule))
            {
                return rule;
            }

            lock (Locker)
            {
                if (ruleSet.TryGetValue(patern, out rule))
                {
                    return rule;
                }

                var matchedRule = _repository.FindRegexMatch(patern);

                // even if mactedRule is NULL - we save it to cache - to avoid searching for this NULL again
                ruleSet.Add(patern, matchedRule);
                return matchedRule;
            }
        }

        public RedirectRuleModel FindExactMatch(string patern)
        {
            if (patern == null)
            {
                return null;
            }

            _cache.TryGet<Dictionary<string, RedirectRuleModel>>(CacheKeyExact, out var ruleSet);
            if (ruleSet == null)
            {
                ruleSet = new Dictionary<string, RedirectRuleModel>();
                _cache.Add(CacheKeyExact, ruleSet);
            }

            if (ruleSet.TryGetValue(patern, out var rule))
            {
                return rule;
            }

            lock (Locker)
            {
                if (ruleSet.TryGetValue(patern, out rule))
                {
                    return rule;
                }

                var matchedRule = _repository.FindExactMatch(patern);

                // even if mactedRule is NULL - we save it to cache - to avoid searching for this NULL again
                ruleSet.Add(patern, matchedRule);
                return matchedRule;
            }
        }

        public RedirectRuleModel Add(RedirectRuleModel redirectRule)
        {
            var rule = _repository.Add(redirectRule);
            ClearCache();
            return rule;
        }

        public RedirectRuleModel Update(RedirectRuleModel redirectRule)
        {
            var rule = _repository.Update(redirectRule);
            ClearCache();
            return rule;
        }

        public bool Delete(Guid id)
        {
            var result = _repository.Delete(id);
            ClearCache();
            return result;
        }

        public bool ClearAll()
        {
            var result = _repository.ClearAll();
            ClearCache();
            return result;
        }

        private void ClearCache()
        {
            lock (Locker)
            {
                _cache.Remove(CacheKeyRegex);
                _cache.Remove(CacheKeyExact);
                _cache.RemoveByMasterKey(CacheRedirectResolverDecorator.CacheMasterKey);
            }
        }
    }
}
