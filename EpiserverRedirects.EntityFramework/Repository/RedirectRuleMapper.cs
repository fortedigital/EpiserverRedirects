using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System;


namespace Forte.EpiserverRedirects.EntityFramework.Repository
{
    public interface IRedirectRuleMapper
    {
        RedirectRuleEntity ToNewEntity(IRedirectRule item);
        void MapForUpdate(IRedirectRule from, RedirectRuleEntity to);
    }

    public class RedirectRuleMapper : IRedirectRuleMapper
    {
        public RedirectRuleEntity ToNewEntity(IRedirectRule item)
        {
            return new RedirectRuleEntity
            {
                RuleId = Guid.NewGuid(),
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

        public void MapForUpdate(IRedirectRule from, RedirectRuleEntity to)
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
