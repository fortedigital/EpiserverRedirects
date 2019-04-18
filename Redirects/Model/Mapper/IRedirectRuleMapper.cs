using System.Collections.Generic;
using System.Linq;

namespace Forte.RedirectMiddleware.Model.Mapper
{
    public interface IRedirectRuleMapper
    {
        RedirectRuleDto ModelToDto(RedirectRule source);
        IEnumerable<RedirectRuleDto> ModelToDto(IEnumerable<RedirectRule> source);
        RedirectRule DtoToModel(RedirectRuleDto source);    
        IEnumerable<RedirectRule> DtoToModel(IEnumerable<RedirectRuleDto> source);
    }
    

}