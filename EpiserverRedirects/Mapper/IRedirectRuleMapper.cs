using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Mapper
{
    public interface IRedirectRuleMapper
    {
        RedirectRuleDto ModelToDto(IRedirectRule source);
        IRedirectRule DtoToModel(RedirectRuleDto source);
    }
}
