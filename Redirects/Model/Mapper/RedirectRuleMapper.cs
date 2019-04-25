using System.Collections.Generic;
using System.Linq;
using Forte.RedirectMiddleware.Model.RedirectRule;

namespace Forte.RedirectMiddleware.Model.Mapper
{
    public class RedirectRuleMapper : BaseRedirectRuleMapper
    {
        public RedirectRuleMapper() : base(ModelToDtoDelegate, DtoToModelDelegate)
        {
        }
        

        private static RedirectRuleDto ModelToDtoDelegate(RedirectRule.RedirectRule source)
        {
            var destination = new RedirectRuleDto();
            destination.Id = source.Id;
            destination.NewUrl = source.NewUrl;
            destination.OldPath = source.OldPath.Path.OriginalString;
            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
            destination.CreatedOn = source.CreatedOn;
            return destination;
        }

        public static RedirectRule.RedirectRule DtoToModelDelegate(RedirectRuleDto source)
        {
            var destination = new RedirectRule.RedirectRule();
            destination.Id = source.Id;
            destination.NewUrl = source.NewUrl;
            destination.OldPath = UrlPath.UrlPath.Parse(source.OldPath);
            destination.RedirectType = source.RedirectType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
            destination.CreatedOn = source.CreatedOn;
            return destination;
        }
    }
}