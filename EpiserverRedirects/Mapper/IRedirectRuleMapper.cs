using System;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Mapper
{
    public interface IRedirectRuleMapper
    {
        RedirectRuleDto ModelToDto(RedirectRule source);
        RedirectRule DtoToModel(RedirectRuleDto source);    
    }

    public abstract class BaseRedirectRuleMapper : IRedirectRuleMapper
    {
        protected BaseRedirectRuleMapper(Func<RedirectRule, RedirectRuleDto> modelToDtoDelegate, Func<RedirectRuleDto, RedirectRule> dtoToModelDelegate)
        {
            ModelToDtoDelegate = modelToDtoDelegate;
            DtoToModelDelegate = dtoToModelDelegate;
        }
        
        protected BaseRedirectRuleMapper(Func<RedirectRule, RedirectRuleDto> modelToDto)
        {
            ModelToDtoDelegate = modelToDto;
        }
        
        protected BaseRedirectRuleMapper(Func<RedirectRuleDto, RedirectRule> dtoToModel)
        {
            DtoToModelDelegate = dtoToModel;
        }

        private Func<RedirectRule, RedirectRuleDto> ModelToDtoDelegate { get; }
        private Func<RedirectRuleDto, RedirectRule> DtoToModelDelegate { get; }
        public RedirectRuleDto ModelToDto(RedirectRule source)
        {
            var destination = ModelToDtoDelegate(source);
            return destination;
        }
        
        public RedirectRule DtoToModel(RedirectRuleDto source)
        {
            var destination = DtoToModelDelegate(source);
            return destination;
        }
    }
    

}