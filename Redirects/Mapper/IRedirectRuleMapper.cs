using System;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Mapper
{
    public interface IRedirectRuleMapper
    {
        RedirectRuleDto ModelToDto(Model.RedirectRule.RedirectRule source);
        Model.RedirectRule.RedirectRule DtoToModel(RedirectRuleDto source);    
    }

    public abstract class BaseRedirectRuleMapper : IRedirectRuleMapper
    {
        protected BaseRedirectRuleMapper(Func<Model.RedirectRule.RedirectRule, RedirectRuleDto> modelToDtoDelegate, Func<RedirectRuleDto, Model.RedirectRule.RedirectRule> dtoToModelDelegate)
        {
            ModelToDtoDelegate = modelToDtoDelegate;
            DtoToModelDelegate = dtoToModelDelegate;
        }
        
        protected BaseRedirectRuleMapper(Func<Model.RedirectRule.RedirectRule, RedirectRuleDto> modelToDto)
        {
            ModelToDtoDelegate = modelToDto;
        }
        
        protected BaseRedirectRuleMapper(Func<RedirectRuleDto, Model.RedirectRule.RedirectRule> dtoToModel)
        {
            DtoToModelDelegate = dtoToModel;
        }

        private Func<Model.RedirectRule.RedirectRule, RedirectRuleDto> ModelToDtoDelegate { get; }
        private Func<RedirectRuleDto, Model.RedirectRule.RedirectRule> DtoToModelDelegate { get; }
        public RedirectRuleDto ModelToDto(Model.RedirectRule.RedirectRule source)
        {
            var destination = ModelToDtoDelegate(source);
            return destination;
        }
        
        public Model.RedirectRule.RedirectRule DtoToModel(RedirectRuleDto source)
        {
            var destination = DtoToModelDelegate(source);
            return destination;
        }
    }
    

}