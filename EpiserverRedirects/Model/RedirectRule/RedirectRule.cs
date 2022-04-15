using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;
using EPiServer.Security;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class RedirectRule : IDynamicData
    {
        private string _oldPattern;
        private string _newPattern;

        public Identity Id { get; set; }

        public int? ContentId { get; set; }

        public string OldPattern
        {
            get => UrlPath.EnsurePathEncoding(_oldPattern);
            set => _oldPattern = UrlPath.EnsurePathEncoding(value);
        }

        public string NewPattern
        {
            get => UrlPath.EnsurePathEncoding(_newPattern);
            set => _newPattern = UrlPath.EnsurePathEncoding(value);
        }

        public RedirectRuleType RedirectRuleType { get; set; }

        public RedirectType RedirectType { get; set; }

        public RedirectOrigin RedirectOrigin { get; set; }
        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string Notes { get; set; }
        public int Priority { get; set; }

        public void FromManual()
        {
            CreatedOn = DateTime.UtcNow;
            CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name;
            RedirectOrigin = RedirectOrigin.Manual;
        }

        public static RedirectRule NewFromManual(string oldPattern, string newPattern, RedirectType redirectType,
            RedirectRuleType redirectRuleType, bool isActive, string notes, int priority)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.Manual,
                OldPattern = oldPattern,
                NewPattern = newPattern,
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = isActive,
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                Notes = notes,
                Priority = priority
            };
        }

        public static RedirectRule NewFromSystem(string oldPattern, string newPattern, RedirectType redirectType,
            RedirectRuleType redirectRuleType, string notes, int priority)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.System,
                OldPattern = oldPattern,
                NewPattern = newPattern,
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = true,
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                Notes = notes,
                Priority = priority
            };
        }

        public static RedirectRule NewFromSystem(string oldPattern, int contentId, RedirectType redirectType,
            RedirectRuleType redirectRuleType, string notes, int priority)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.System,
                OldPattern = oldPattern,
                ContentId = contentId,
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = true,
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                Notes = notes,
                Priority = priority
            };
        }

        public static RedirectRule NewFromImport(string oldPattern, string newPattern, RedirectType redirectType,
            RedirectRuleType redirectRuleType, bool isActive, string notes, int priority)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.Import,
                OldPattern = UrlPath.ExtractRelativePath(oldPattern),
                NewPattern = UrlPath.ExtractRelativePath(newPattern),
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = isActive,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                Notes = notes,
                Priority = priority
            };
        }

        public static RedirectRule NewFromImport(string oldPattern, int contentId, RedirectType redirectType,
            RedirectRuleType redirectRuleType, bool isActive, string notes, int priority)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.Import,
                OldPattern = UrlPath.ExtractRelativePath(oldPattern),
                ContentId = contentId,
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = isActive,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                Notes = notes,
                Priority = priority
            };
        }
    }
}
