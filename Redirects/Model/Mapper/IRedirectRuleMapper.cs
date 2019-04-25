using System;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Model.Mapper
{
    public interface IRedirectRuleMapper
    {
        RedirectRuleDto ModelToDto(RedirectRule.RedirectRule source);
        RedirectRule.RedirectRule DtoToModel(RedirectRuleDto source);    
    }

    public abstract class BaseRedirectRuleMapper : IRedirectRuleMapper
    {
        protected BaseRedirectRuleMapper(Func<RedirectRule.RedirectRule, RedirectRuleDto> modelToDtoDelegate, Func<RedirectRuleDto, RedirectRule.RedirectRule> dtoToModelDelegate)
        {
            ModelToDtoDelegate = modelToDtoDelegate;
            DtoToModelDelegate = dtoToModelDelegate;
        }
        
        protected BaseRedirectRuleMapper(Func<RedirectRule.RedirectRule, RedirectRuleDto> modelToDto)
        {
            ModelToDtoDelegate = modelToDto;
        }
        
        protected BaseRedirectRuleMapper(Func<RedirectRuleDto, RedirectRule.RedirectRule> dtoToModel)
        {
            DtoToModelDelegate = dtoToModel;
        }

        private Func<RedirectRule.RedirectRule, RedirectRuleDto> ModelToDtoDelegate { get; }
        private Func<RedirectRuleDto, RedirectRule.RedirectRule> DtoToModelDelegate { get; }
        public RedirectRuleDto ModelToDto(RedirectRule.RedirectRule source)
        {
            var destination = ModelToDtoDelegate(source);
            return destination;
        }
        
        public RedirectRule.RedirectRule DtoToModel(RedirectRuleDto source)
        {
            var destination = DtoToModelDelegate(source);
            return destination;
        }
    }
    

}