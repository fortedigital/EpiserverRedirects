using Forte.EpiserverRedirects.Model.RedirectRule;


namespace Forte.EpiserverRedirects.DynamicData
{
    public interface IDynamicDataRedirectRuleMapper
    {
        DynamicDataRedirectRule ToNewEntity(IRedirectRule item);
        void MapForUpdate(IRedirectRule from, DynamicDataRedirectRule to);
    }
}
