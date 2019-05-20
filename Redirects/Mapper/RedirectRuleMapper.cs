using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.UrlPath;

namespace Forte.Redirects.Mapper
{
    public class RedirectRuleMapper : BaseRedirectRuleMapper
    {
        public RedirectRuleMapper() : base(ModelToDtoDelegate, DtoToModelDelegate)
        {
        }
        

        private static RedirectRuleDto ModelToDtoDelegate(RedirectRule source)
        {
            var destination = new RedirectRuleDto();
            destination.Id = source.Id.ExternalId;
            destination.OldPattern = source.OldPattern;
            destination.NewPattern = source.NewPattern;
            destination.RedirectType = source.RedirectType;
            destination.RedirectRuleType = source.RedirectRuleType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
            destination.CreatedOn = source.CreatedOn;
            destination.CreatedBy = source.CreatedBy;

            return destination;
        }

        private static RedirectRule DtoToModelDelegate(RedirectRuleDto source)
        {
            var destination = new RedirectRule();
            destination.Id = source.Id;
            destination.OldPattern = UrlPath.NormalizePath(source.OldPattern);
            destination.NewPattern = source.NewPattern;
            destination.RedirectType = source.RedirectType;
            destination.RedirectRuleType = source.RedirectRuleType;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
            destination.CreatedOn = source.CreatedOn;
            destination.CreatedBy = source.CreatedBy;

            return destination;
        }
    }
}