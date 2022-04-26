using System;
using System.Linq;
using EPiServer.Data;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace EpiserverRedirects.EntityFramework.Repository
{
    public class EntityFrameworkRepository<TDbContext> : IRedirectRuleRepository where TDbContext : RedirectRulesDbContext
    {
        private readonly TDbContext _dbContext;

        public EntityFrameworkRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RedirectRule GetById(Guid id)
        {
            return _dbContext.RedirectRules.First(rule => rule.Id.ExternalId == id);
        }

        public IQueryable<RedirectRule> GetAll()
        {
            // TODO: Introduce entity and map to model maybe? Is that possible with IQueryable?
            return _dbContext.RedirectRules;
        }

        public RedirectRule Add(RedirectRule redirectRule)
        {
            // TODO: It would be better to not reference Episerver here
            redirectRule.Id = Identity.NewIdentity();

            var entry = _dbContext.RedirectRules.Add(redirectRule);
            SaveChanges();

            return entry.Entity;
        }

        public RedirectRule Update(RedirectRule redirectRule)
        {
            var entry = _dbContext.RedirectRules.Update(redirectRule);
            SaveChanges();

            return entry.Entity;
        }

        public bool Delete(Guid id)
        {
            try
            {
                var entity = GetById(id);
                _dbContext.RedirectRules.Remove(entity);
                SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ClearAll()
        {
            try
            {
                // EF does not have anything like `DeleteAll` so we have to use `RemoveRange`.
                // Since some databases can have dozens of thousands of rules it could kill the SQL process or cause OOM because of an in-memory cache of tracked entities.
                // To mitigate that, we are calling `RemoveRange` in batches until we clear all records. Not great, not terrible...
                const int maxItemsToRemoveInBatch = 1000;

                while (_dbContext.RedirectRules.Any())
                {
                    var batchOfItemsToRemove = _dbContext.RedirectRules.Take(maxItemsToRemoveInBatch);
                    _dbContext.RedirectRules.RemoveRange(batchOfItemsToRemove);
                    SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
