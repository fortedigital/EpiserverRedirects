using System;
using Forte.Redirects.Model;
using Forte.Redirects.Model.RedirectRule;

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
            destination.ContentId = source.ContentId;
            destination.RedirectType = source.RedirectType;
            destination.RedirectRuleType = source.RedirectRuleType;
            destination.RedirectOrigin = source.RedirectOrigin;
            destination.IsActive = source.IsActive;
            destination.Notes = source.Notes;
            destination.CreatedOn = DateTime.SpecifyKind(source.CreatedOn, DateTimeKind.Utc);
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

            return destination;
        }
    }
}