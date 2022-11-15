using EPiServer.Data;
using EPiServer.Data.Entity;
using Forte.EpiserverRedirects.Model.RedirectRule;


namespace Forte.EpiserverRedirects.DynamicDataStore
{
    public interface IDdsRedirectRuleMapper
    {
        DdsRedirectRule MapForSave(RedirectRuleModel item);
        void MapForUpdate(RedirectRuleModel from, DdsRedirectRule to);
        RedirectRuleModel MapToModel(DdsRedirectRule entity);
    }

    public class DdsRedirectRuleMapper : IDdsRedirectRuleMapper
    {
        public DdsRedirectRule MapForSave(RedirectRuleModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new DdsRedirectRule
            {
                Id = Identity.NewIdentity(),
                ContentId = model.ContentId,
                OldPattern = model.OldPattern,
                NewPattern = model.NewPattern,
                RedirectRuleType = model.RedirectRuleType,
                RedirectType = model.RedirectType,
                RedirectOrigin = model.RedirectOrigin,
                CreatedOn = model.CreatedOn,
                IsActive = model.IsActive,
                CreatedBy = model.CreatedBy,
                Notes = model.Notes,
                Priority = model.Priority
            };
        }

        public void MapForUpdate(RedirectRuleModel from, DdsRedirectRule to)
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

        public RedirectRuleModel MapToModel(DdsRedirectRule entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RedirectRuleModel
            {
                Id = entity.Id.ExternalId,
                ContentId = entity.ContentId,
                OldPattern = entity.OldPattern,
                NewPattern = entity.NewPattern,
                RedirectRuleType = entity.RedirectRuleType,
                RedirectType = entity.RedirectType,
                RedirectOrigin = entity.RedirectOrigin,
                CreatedOn = entity.CreatedOn,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                Notes = entity.Notes,
                Priority = entity.Priority
            };
        }
    }
}
