using System;
using Forte.Redirects.Mapper;
using Forte.Redirects.Model.RedirectRule;

namespace RedirectTests.Mapper
{
    public class RedirectRuleTestMapper : BaseRedirectRuleMapper
    {
        public RedirectRuleTestMapper(
            Func<RedirectRule, RedirectRuleDto> modelToDto,
            Func<RedirectRuleDto, RedirectRule> dtoToModel)
            : base(modelToDto, dtoToModel){ }
        
        public RedirectRuleTestMapper(Func<RedirectRule, RedirectRuleDto> modelToDto) : base(modelToDto){ }

    }
}