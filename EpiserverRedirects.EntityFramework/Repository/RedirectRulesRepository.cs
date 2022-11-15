using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace Forte.EpiserverRedirects.EntityFramework.Repository
{
    public class RedirectRulesRepository<TDbContext> : IRedirectRuleRepository
        where TDbContext : IRedirectRulesDbContext
    {
        private readonly IRedirectRulesDbContext _dbContext;
        private readonly IRedirectRuleMapper _mapper;

        public RedirectRulesRepository(
            IRedirectRulesDbContext dbContext,
            IRedirectRuleMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public RedirectRuleModel GetById(Guid id)
        {
            var entity = _dbContext.RedirectRules.FirstOrDefault(rule => rule.Id == id);
            return _mapper.MapToModel(entity);
        }

        public IList<RedirectRuleModel> GetAll()
        {
            return _dbContext.RedirectRules
                .Select(entity => _mapper.MapToModel(entity))
                .ToList();
        }

        public IList<RedirectRuleModel> Query(out int allRedirectsCount, RedirectRuleQuery query)
        {
            return _dbContext.RedirectRules
                .ApplyQuery(out allRedirectsCount, query)
                .Select(entity => _mapper.MapToModel(entity))
                .ToList();
        }

        public IList<RedirectRuleModel> GetByContent(IList<int> contentIds)
        {
            return _dbContext.RedirectRules
                .Where(x => x.ContentId.HasValue && contentIds.Contains(x.ContentId.Value))
                .Select(entity => _mapper.MapToModel(entity))
                .ToList();
        }

        public RedirectRuleModel FindRegexMatch(string patern)
        {
            var entity = _dbContext.RedirectRules
                .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.Regex)
                .OrderBy(x => x.Priority)
                .AsEnumerable()
                .FirstOrDefault(r => Regex.IsMatch(patern, r.OldPattern, RegexOptions.IgnoreCase));
            return _mapper.MapToModel(entity);
        }

        public RedirectRuleModel FindExactMatch(string patern)
        {
            var entity = _dbContext.RedirectRules
                .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.ExactMatch)
                .OrderBy(x => x.Priority)
                .FirstOrDefault(r => r.OldPattern == patern);
            return _mapper.MapToModel(entity);
        }

        public RedirectRuleModel Add(RedirectRuleModel redirectRule)
        {
            var entity = _mapper.MapForSave(redirectRule);
            _dbContext.RedirectRules.Add(entity);
            _dbContext.SaveChanges();
            return _mapper.MapToModel(entity);
        }

        public RedirectRuleModel Update(RedirectRuleModel redirectRule)
        {
            var entity = _dbContext.RedirectRules.FirstOrDefault(rule => rule.Id == redirectRule.Id);
            if (entity == null)
            {
                throw new ArgumentException("No existing redirect with this GUID");
            }

            _mapper.MapForUpdate(redirectRule, entity);
            _dbContext.RedirectRules.Update(entity);
            _dbContext.SaveChanges();
            return _mapper.MapToModel(entity);
        }

        public bool Delete(Guid id)
        {
            try
            {
                var entity = _dbContext.RedirectRules.FirstOrDefault(rule => rule.Id == id);
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
