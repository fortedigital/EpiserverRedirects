using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using System;


namespace Forte.EpiserverRedirects.EntityFramework.Repository
{
    public class RedirectRuleMapper : IRedirectRuleMapper<RedirectRuleEntity>
    {
        public RedirectRuleEntity ToNewEntity(IRedirectRule item)
        {
            var entity = new RedirectRuleEntity { RuleId = Guid.NewGuid() };
            RedirectRuleStoreMapper.MapForCreate(item, entity);
            return entity;
        }

        public void MapForUpdate(IRedirectRule from, RedirectRuleEntity to)
        {
            RedirectRuleStoreMapper.MapForUpdate(from, to);
        }
    }
}
