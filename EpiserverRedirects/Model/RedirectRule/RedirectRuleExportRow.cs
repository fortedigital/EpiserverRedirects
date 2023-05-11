using System;
using CsvHelper.Configuration.Attributes;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    public class RedirectRuleExportRow
    {
        [Index(0)]
        public string OldPattern { get; set; }
        
        [Index(1)]
        public string NewPattern { get; set; }

        [Index(2), Optional]
        public int? ContentId { get; set; }
        
        [Index(3)]
        public string RedirectRuleType { get; set; }
        
        [Index(4)]
        public string RedirectType { get; set; }
        
        [Index(5), Optional]
        public byte? Priority { get; set; }

        [Index(6)]
        public string IsActive { get; set; }

        [Index(7)]
        public string Notes { get; set; }
        
        [Index(8)]
        public string RedirectOrigin { get; set; }
        
        [Index(9)]
        public string CreatedOn { get; set; }
        
        [Index(10)]
        public string CreatedBy { get; set; }
        
        [Index(11), Optional]
        public Guid? Host { get; set; }
        
        public static RedirectRuleExportRow CreateFromRedirectRule(IRedirectRule redirectRule)
        {
            return new RedirectRuleExportRow
            {
                OldPattern = redirectRule.OldPattern,
                NewPattern = redirectRule.NewPattern,
                ContentId = redirectRule.ContentId,
                RedirectType = redirectRule.RedirectType.ToString(),
                RedirectRuleType = redirectRule.RedirectRuleType.ToString(),
                Priority = Convert.ToByte(redirectRule.Priority),
                RedirectOrigin = redirectRule.RedirectOrigin.ToString(),
                IsActive = redirectRule.IsActive.ToString().ToUpper(),
                CreatedOn = redirectRule.CreatedOn.ToString("dd/MM/yyyy H:mm:ss"),
                CreatedBy = redirectRule.CreatedBy,
                Notes = redirectRule.Notes,
                Host = redirectRule.HostId
            };
        }
    }
}