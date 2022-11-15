using EPiServer.Data;
using Forte.EpiserverRedirects.Model.RedirectRule;


namespace Forte.EpiserverRedirects.DynamicData
{
    public class DynamicDataRedirectRuleMapper : IDynamicDataRedirectRuleMapper
    {
        public DynamicDataRedirectRule ToNewEntity(IRedirectRule item)
        {
            return new DynamicDataRedirectRule
            {
                Id = Identity.NewIdentity(),
                ContentId = item.ContentId,
                OldPattern = item.OldPattern,
                NewPattern = item.NewPattern,
                RedirectRuleType = item.RedirectRuleType,
                RedirectType = item.RedirectType,
                RedirectOrigin = item.RedirectOrigin,
                CreatedOn = item.CreatedOn,
                IsActive = item.IsActive,
                CreatedBy = item.CreatedBy,
                Notes = item.Notes,
                Priority = item.Priority
            };
        }

        public void MapForUpdate(IRedirectRule from, DynamicDataRedirectRule to)
        {
            to.OldPattern = from.OldPattern;
            to.NewPattern = from.NewPattern;
            to.RedirectType = from.RedirectType;
            to.RedirectRuleType = from.RedirectRuleType;
            to.RedirectOrigin = from.RedirectOrigin;
            to.IsActive = from.IsActive;
            to.Notes = from.Notes;
            to.Priority = from.Priority;
            to.ContentId = from.ContentId;
        }
    }
}
