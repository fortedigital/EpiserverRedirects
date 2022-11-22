using EPiServer.Data;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System.Linq;

namespace Forte.EpiserverRedirects.DynamicData
{
    public class DynamicDataRedirectRuleMapper : IDynamicDataRedirectRuleMapper
    {
        public DynamicDataRedirectRule MapForSave(RedirectRuleModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new DynamicDataRedirectRule
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

        public void MapForUpdate(RedirectRuleModel from, DynamicDataRedirectRule to)
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

        public RedirectRuleModel MapToModel(DynamicDataRedirectRule entity)
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

        public SearchResult<RedirectRuleModel> MapSearchResult(SearchResult<DynamicDataRedirectRule> result)
        {
            return new SearchResult<RedirectRuleModel>
            {
                Total = result.Total,
                Items = result.Items
                    .Select(entity => MapToModel(entity))
                    .ToList()
            };
        }
    }
}
