using System;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Mapper
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
            destination.Priority = source.Priority;

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
            destination.Priority = source.Priority;

            return destination;
        }
    }
}