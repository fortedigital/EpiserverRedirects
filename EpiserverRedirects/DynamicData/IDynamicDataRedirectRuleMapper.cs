using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.DynamicData
{
    public interface IDynamicDataRedirectRuleMapper
    {
        DynamicDataRedirectRule MapForSave(RedirectRuleModel item);
        void MapForUpdate(RedirectRuleModel from, DynamicDataRedirectRule to);
        RedirectRuleModel MapToModel(DynamicDataRedirectRule entity);
        SearchResult<RedirectRuleModel> MapSearchResult(SearchResult<DynamicDataRedirectRule> result);
    }
}
