using System;
using System.Linq;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;


namespace Forte.EpiserverRedirects.Caching
{
    public class RedirectRuleCachedRepositoryDecorator : IRedirectRuleRepository
    {
        private readonly ICache _cache;
        private readonly IRedirectRuleRepository _repository;

        public RedirectRuleCachedRepositoryDecorator(ICache cache, IRedirectRuleRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public IRedirectRule GetById(Guid id) => _repository.GetById(id);

        public IQueryable<IRedirectRule> GetAll() => _repository.GetAll();

        public IRedirectRule Add(IRedirectRule redirectRule)
        {
            var rule = _repository.Add(redirectRule);
            ClearCache();
            return rule;
        }

        public IRedirectRule Update(IRedirectRule redirectRule)
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
            _cache.RemoveByMasterKey(CacheRedirectResolverDecorator.CacheMasterKey);
        }
    }
}
