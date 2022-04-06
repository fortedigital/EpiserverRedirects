using System;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Mapper
{
    public class RedirectRuleMapper : IRedirectRuleMapper
    {
        private readonly RedirectsOptions _options;

        public RedirectRuleMapper(RedirectsOptions options)
        {
            _options = options;
        }

        public RedirectRuleDto ModelToDto(RedirectRule source)
        {
            var destination = new RedirectRuleDto
            {
                Id = source.Id.ExternalId,
                OldPattern = source.OldPattern,
                NewPattern = source.NewPattern,
                ContentId = source.ContentId,
                RedirectType = source.RedirectType,
                RedirectRuleType = source.RedirectRuleType,
                RedirectOrigin = source.RedirectOrigin,
                IsActive = source.IsActive,
                Notes = source.Notes,
                CreatedOn = DateTime.SpecifyKind(source.CreatedOn, DateTimeKind.Utc),
                CreatedBy = source.CreatedBy,
                Priority = source.Priority
            };

            return destination;
        }

        public RedirectRule DtoToModel(RedirectRuleDto source)
        {
            var destination = new RedirectRule
            {
                Id = source.Id,
                OldPattern = UrlPath.ExtractRelativePath(source.OldPattern),
                NewPattern = UrlPath.ExtractRelativePath(source.NewPattern),
                RedirectType = source.RedirectType,
                RedirectRuleType = source.RedirectRuleType,
                IsActive = source.IsActive,
                Notes = source.Notes,
                ContentId = source.ContentId,
                Priority = (source.Priority.HasValue && source.Priority > 0) ? source.Priority.Value : _options.DefaultRedirectRulePriority
            };

            return destination;
        }
    }
}
