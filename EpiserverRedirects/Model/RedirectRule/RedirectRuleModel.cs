using EPiServer.Security;
using System;


namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    public class RedirectRuleModel : IRedirectRule
    {
        public Guid RuleId { get; set; }
        public int? ContentId { get; set; }
        public string OldPattern { get; set; }
        public string NewPattern { get; set; }
        public RedirectRuleType RedirectRuleType { get; set; }
        public RedirectType RedirectType { get; set; }
        public RedirectOrigin RedirectOrigin { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string Notes { get; set; }
        public int Priority { get; set; }

        public static void FromManual(IRedirectRule item)
        {
            item.CreatedOn = DateTime.UtcNow;
            item.CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name;
            item.RedirectOrigin = RedirectOrigin.Manual;
        }

        public static RedirectRuleModel NewFromManual(string oldPattern, string newPattern, RedirectType redirectType,
            RedirectRuleType redirectRuleType, bool isActive, string notes, int priority)
        {
            return new RedirectRuleModel
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

        public static RedirectRuleModel NewFromSystem(string oldPattern, string newPattern, RedirectType redirectType,
            RedirectRuleType redirectRuleType, string notes, int priority)
        {
            return new RedirectRuleModel
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

        public static RedirectRuleModel NewFromSystem(string oldPattern, int contentId, RedirectType redirectType,
            RedirectRuleType redirectRuleType, string notes, int priority)
        {
            return new RedirectRuleModel
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

        public static RedirectRuleModel NewFromImport(string oldPattern, string newPattern, RedirectType redirectType,
            RedirectRuleType redirectRuleType, bool isActive, string notes, int priority)
        {
            return new RedirectRuleModel
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

        public static RedirectRuleModel NewFromImport(string oldPattern, int contentId, RedirectType redirectType,
            RedirectRuleType redirectRuleType, bool isActive, string notes, int priority)
        {
            return new RedirectRuleModel
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
