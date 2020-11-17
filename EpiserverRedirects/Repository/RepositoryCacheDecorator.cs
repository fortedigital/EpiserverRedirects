using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Data.Dynamic;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.System;

namespace Forte.EpiserverRedirects.Repository
{
    public class RepositoryCacheDecorator : DynamicDataStoreRepository
    {
        public const string CacheKey = "Forte.EpiserverRedirects.RedirectRuleList";

        public RepositoryCacheDecorator(DynamicDataStoreFactory dynamicDataStoreFactory, ICache<IQueryable<RedirectRule>> cache) : base(
            dynamicDataStoreFactory)
        {
            Items = new CachedQueryable<RedirectRule>(dynamicDataStoreFactory, cache, CacheKey);
        }

        public override RedirectRule GetById(Guid id)=> this.FirstOrDefault(r => r.Id.ExternalId == id);

        public override IEnumerable<RedirectRule> GetAll() => this;

        private class CachedQueryable<T> : IQueryable<T> where T : class
        {
            private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;
            private readonly ICache<IQueryable<T>> _cache;
            private readonly string _cacheKey;

            public CachedQueryable(DynamicDataStoreFactory dynamicDataStoreFactory, ICache<IQueryable<T>> cache, string cacheKey)
            {
                _dynamicDataStoreFactory = dynamicDataStoreFactory ?? throw new ArgumentNullException(nameof(dynamicDataStoreFactory));
                _cache = cache ?? throw new ArgumentNullException(nameof(cache));
                _cacheKey = cacheKey ?? throw new ArgumentNullException(nameof(cacheKey));
            }

            private IQueryable<T> GetItems()
            {
                if (_cache.TryGet(_cacheKey, out var queryable))
                {
                    return queryable;
                }

                queryable = _dynamicDataStoreFactory.CreateStore(typeof(T)).Items<T>().ToList().AsQueryable();
                _cache.Add(_cacheKey, queryable);
                return queryable;
            }


            public IEnumerator<T> GetEnumerator()
            {
                return GetItems().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public Expression Expression => GetItems().Expression;
            public Type ElementType => GetItems().ElementType;
            public IQueryProvider Provider => GetItems().Provider;
        }
    }
}