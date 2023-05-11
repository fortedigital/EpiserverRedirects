using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Web;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Mapper
{
    public class RedirectRuleModelMapper : IRedirectRuleModelMapper
    {
        private readonly RedirectsOptions _options;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private IEnumerable<SiteDefinition> _allHosts;

        public IEnumerable<SiteDefinition> AllHosts
        {
            get { return _allHosts ??= _siteDefinitionRepository.List(); }
        }

        public RedirectRuleModelMapper(RedirectsOptions options, ISiteDefinitionRepository siteDefinitionRepository)
        {
            _options = options;
           _siteDefinitionRepository = siteDefinitionRepository;
        }

        public RedirectRuleDto ModelToDto(IRedirectRule source)
        {
            var destination = new RedirectRuleDto
            {
                Id = source.RuleId,
                OldPattern = UrlPath.EnsurePathEncoding(source.OldPattern),
                NewPattern = UrlPath.EnsurePathEncoding(source.NewPattern),
                ContentId = source.ContentId,
                RedirectType = source.RedirectType,
                RedirectRuleType = source.RedirectRuleType,
                RedirectOrigin = source.RedirectOrigin,
                IsActive = source.IsActive,
                Notes = source.Notes,
                CreatedOn = DateTime.SpecifyKind(source.CreatedOn, DateTimeKind.Utc),
                CreatedBy = source.CreatedBy,
                Priority = source.Priority,
                HostId = source.HostId,
                HostName = GetHostNameByHostId(source.HostId)
            };

            return destination;
        }

        public IRedirectRule DtoToModel(RedirectRuleDto source)
        {
            var destination = new RedirectRuleModel
            {
                RuleId = source.Id.GetValueOrDefault(),
                OldPattern = UrlPath.EnsurePathEncoding(UrlPath.ExtractRelativePath(source.OldPattern)),
                NewPattern = UrlPath.EnsurePathEncoding(UrlPath.NormalizeNewPath(source.NewPattern)),
                RedirectType = source.RedirectType,
                RedirectRuleType = source.RedirectRuleType,
                IsActive = source.IsActive,
                Notes = source.Notes,
                ContentId = source.ContentId,
                Priority = (source.Priority.HasValue && source.Priority > 0) ? source.Priority.Value : _options.DefaultRedirectRulePriority,
                HostId = source.HostId
            };

            return destination;
        }

        private string GetHostNameByHostId(Guid? hostId)
        {
            return hostId is null
                ? "All hosts"
                : AllHosts.Where(s => s.Id == hostId).Select(s => s.Name).FirstOrDefault();
        }
    }
}
