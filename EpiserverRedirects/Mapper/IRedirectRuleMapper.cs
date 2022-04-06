using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Mapper
{
    public interface IRedirectRuleMapper
    {
        RedirectRuleDto ModelToDto(RedirectRule source);
        RedirectRule DtoToModel(RedirectRuleDto source);
    }
}
