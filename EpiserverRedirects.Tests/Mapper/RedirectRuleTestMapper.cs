using System;
using Forte.EpiserverRedirects.Mapper;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Tests.Mapper
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