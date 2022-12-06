using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using System;
using System.Linq;


namespace Forte.EpiserverRedirects.EntityFramework.Repository
{
    public class RedirectRulesRepository : IRedirectRuleRepository
    {
        private readonly IRedirectRulesDbContext _dbContext;
        private readonly IRedirectRuleMapper<RedirectRuleEntity> _mapper;

        public RedirectRulesRepository(
            IRedirectRulesDbContext dbContext,
            IRedirectRuleMapper<RedirectRuleEntity> mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IQueryable<IRedirectRule> GetAll()
        {
            return _dbContext.RedirectRules;
        }

        public IRedirectRule GetById(Guid id)
        {
            return _dbContext.RedirectRules.First(rule => rule.RuleId == id);
        }

        public IRedirectRule Add(IRedirectRule redirectRule)
        {
            var entity = _mapper.ToNewEntity(redirectRule);
            _dbContext.RedirectRules.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public IRedirectRule Update(IRedirectRule redirectRule)
        {
            var entity = _dbContext.RedirectRules.FirstOrDefault(rule => rule.RuleId == redirectRule.RuleId);
            if (entity == null)
            {
                throw new ArgumentException("No existing redirect with this GUID");
            }

            _mapper.MapForUpdate(redirectRule, entity);
            _dbContext.RedirectRules.Update(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public bool Delete(Guid id)
        {
            try
            {
                var entity = _dbContext.RedirectRules.FirstOrDefault(rule => rule.RuleId == id);
                if (entity == null)
                {
                    return true;
                }

                _dbContext.RedirectRules.Remove(entity);
                _dbContext.SaveChanges();
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
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
