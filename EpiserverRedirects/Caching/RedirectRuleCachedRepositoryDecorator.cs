using System;
using System.Linq;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Caching
{
    public class RedirectRuleCachedRepositoryDecorator : IRedirectRuleRepository
    {
        public const string CacheKey = "Forte.EpiserverRedirects.RedirectRuleList";
        private readonly ICache _cache;
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private static readonly object Locker = new object();

        public RedirectRuleCachedRepositoryDecorator(IRedirectRuleRepository redirectRuleRepository, ICache cache)
        {
            _cache = cache;
            _redirectRuleRepository = redirectRuleRepository;
        }

        public RedirectRule GetById(Guid id) => _redirectRuleRepository.GetById(id);

        public IQueryable<RedirectRule> GetAll()
        {
            if (_cache.TryGet<RedirectRule[]>(CacheKey, out var redirectRulesFirstAttempt))
            {
                return redirectRulesFirstAttempt.AsQueryable();
            }

            lock (Locker)
            {
                if (_cache.TryGet<RedirectRule[]>(CacheKey, out var redirectRules))
                {
                    return redirectRules.AsQueryable();
                }

                redirectRules = _redirectRuleRepository.GetAll().ToArray();
                _cache.Add(CacheKey, redirectRules);

                return redirectRules.AsQueryable();
            }
        }

        public RedirectRule Add(RedirectRule redirectRule)
        {
            var rule = _redirectRuleRepository.Add(redirectRule);
            ClearCache();

            return rule;
        }

        public RedirectRule Update(RedirectRule redirectRule)
        {
            var rule = _redirectRuleRepository.Update(redirectRule);
            ClearCache();

            return rule;
        }

        public bool Delete(Guid id)
        {
            var result = _redirectRuleRepository.Delete(id);
            ClearCache();

            return result;
        }

        public bool ClearAll()
        {
            var result = _redirectRuleRepository.ClearAll();
            ClearCache();

            return result;
        }

        private void ClearCache()
        {
            _cache.Remove(CacheKey);
            _cache.RemoveByRegion(CacheRedirectResolverDecorator.CacheRegionKey);
        }
    }
}
