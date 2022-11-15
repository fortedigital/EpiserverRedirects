using EPiServer.Data;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;


namespace Forte.EpiserverRedirects.DynamicData
{
    public class DynamicDataRedirectRuleMapper : IRedirectRuleMapper<RedirectRule>
    {
        public RedirectRule ToNewEntity(IRedirectRule item)
        {
            var entity = new RedirectRule { Id = Identity.NewIdentity() };
            RedirectRuleStoreMapper.MapForCreate(item, entity);
            return entity;
        }

        public void MapForUpdate(IRedirectRule from, RedirectRule to)
        {
            RedirectRuleStoreMapper.MapForUpdate(from, to);
        }
    }
}
