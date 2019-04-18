using System.Collections.Generic;
using System.Linq;

namespace Forte.RedirectMiddleware.Model.Mapper
{
    public class RedirectRuleMapper : IRedirectRuleMapper
    {
        private static void ModelToDto(RedirectRule source, RedirectRuleDto destination)
        {
            destination.Id = source.Id;
            destination.NewUrl = source.NewUrl;
            destination.OldPath = source.OldPath.Path.OriginalString;
            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
        }
        
        public RedirectRuleDto ModelToDto(RedirectRule source)
        {
            var destination = new RedirectRuleDto();
            ModelToDto(source, destination);
            return destination;
        }

        public IEnumerable<RedirectRuleDto> ModelToDto(IEnumerable<RedirectRule> source)
        {
            return source.Select(ModelToDto);
        }

        //TODO: zwracac boolean z TryCreate lub opakowac w try catche
        public static void DtoToModel(RedirectRuleDto source, RedirectRule destination)
        {
            destination.Id = source.Id;
            destination.NewUrl = source.NewUrl;
            destination.OldPath = UrlPath.Create(source.OldPath);
            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
        }

        public RedirectRule DtoToModel(RedirectRuleDto source)
        {
            var destination = new RedirectRule();
            DtoToModel(source, destination);
            return destination;
        }
        
        public IEnumerable<RedirectRule> DtoToModel(IEnumerable<RedirectRuleDto> source)
        {
            return source.Select(DtoToModel);
        }
    }
}