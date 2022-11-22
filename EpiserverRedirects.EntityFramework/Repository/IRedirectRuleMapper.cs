using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.EntityFramework.Repository
{
    public interface IRedirectRuleMapper
    {
        RedirectRuleEntity MapForSave(RedirectRuleModel item);

        void MapForUpdate(RedirectRuleModel from, RedirectRuleEntity to);

        RedirectRuleModel MapToModel(RedirectRuleEntity entity);

        SearchResult<RedirectRuleModel> MapSearchResult(SearchResult<RedirectRuleEntity> result);
    }
}
