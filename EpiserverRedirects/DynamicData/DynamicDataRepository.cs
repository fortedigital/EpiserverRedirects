using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Forte.EpiserverRedirects.DynamicData
{
    public class DynamicDataRepository : IRedirectRuleRepository
    {
        private readonly IDynamicDataStore<RedirectRule> _ruleStore;
        private readonly IRedirectRuleMapper<RedirectRule> _mapper;

        public DynamicDataRepository(
            IDynamicDataStore<RedirectRule> ruleStore,
            IRedirectRuleMapper<RedirectRule> mapper)
        {
            _ruleStore = ruleStore;
            _mapper = mapper;
        }

        public IQueryable<IRedirectRule> GetAll()
        {
            return _ruleStore.Items();
        }

        public IRedirectRule GetById(Guid id)
        {
            return _ruleStore.GetById(id);
        }

        public IRedirectRule Add(IRedirectRule redirectRule)
        {
            var entity = _mapper.ToNewEntity(redirectRule);
            _ruleStore.Save(entity);
            return entity;
        }

        public void AddRange(IEnumerable<IRedirectRule> redirectRules)
        {
            foreach (var redirectRule in redirectRules)
            {
                var entity = _mapper.ToNewEntity(redirectRule);
                _ruleStore.Save(entity);
            }
        }

        public IRedirectRule Update(IRedirectRule redirectRule)
        {
            var entity = _ruleStore.GetById(redirectRule.RuleId);
            if (entity == null)
            {
                throw new InvalidOperationException("No existing redirect with this GUID");
            }

            _mapper.MapForUpdate(redirectRule, entity);
            _ruleStore.Save(entity);
            return entity;
        }

        public bool Delete(Guid id)
        {
            try
            {
                _ruleStore.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ClearAll()
        {
            _ruleStore.DeleteAll();
            return true;
        }
    }
}
