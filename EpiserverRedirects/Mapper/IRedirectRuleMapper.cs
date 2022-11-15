using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Mapper
{
    public interface IRedirectRuleMapper
    {
        RedirectRuleDto ModelToDto(RedirectRuleModel source);
        RedirectRuleModel DtoToModel(RedirectRuleDto source);
    }
}
