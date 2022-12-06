using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Mapper
{
    public interface IRedirectRuleModelMapper
    {
        RedirectRuleDto ModelToDto(IRedirectRule source);
        IRedirectRule DtoToModel(RedirectRuleDto source);
    }
}
