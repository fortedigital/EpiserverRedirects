using System;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Model.Mapper
{
    public class RedirectRuleTestMapper : BaseRedirectRuleMapper
    {
        public RedirectRuleTestMapper(
            Func<RedirectRule.RedirectRule, RedirectRuleDto> modelToDto,
            Func<RedirectRuleDto, RedirectRule.RedirectRule> dtoToModel)
            : base(modelToDto, dtoToModel){ }
        
        public RedirectRuleTestMapper(Func<RedirectRule.RedirectRule, RedirectRuleDto> modelToDto) : base(modelToDto){ }

    }
}