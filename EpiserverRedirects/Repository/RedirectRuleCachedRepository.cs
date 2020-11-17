using System.Linq;
using EPiServer.Data.Dynamic;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.System;

namespace Forte.EpiserverRedirects.Repository
{
    public class RedirectRuleCachedRepository : DynamicDataStoreRepository
    {
        public const string CacheKey = "Forte.EpiserverRedirects.RedirectRuleList";
        private readonly ICache<IQueryable<RedirectRule>> _cache;

        public RedirectRuleCachedRepository(DynamicDataStoreFactory dynamicDataStoreFactory, ICache<IQueryable<RedirectRule>> cache) : 
            base(dynamicDataStoreFactory)
        {
            _cache = cache;
        }
        public override IQueryable<RedirectRule> GetAll()
        {
            if (_cache.TryGet(CacheKey, out var queryable))
            {
                return queryable;
            }

            queryable = base.GetAll().ToList().AsQueryable();
            _cache.Add(CacheKey, queryable);
            return queryable;
        }
    }
}
