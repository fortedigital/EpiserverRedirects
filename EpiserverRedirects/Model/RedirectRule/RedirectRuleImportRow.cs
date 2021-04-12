using System;
using System.Linq;
using Castle.Core.Internal;
using CsvHelper.Configuration.Attributes;
using Settings = Forte.EpiserverRedirects.Configuration.Configuration;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    public class RedirectRuleImportRow
    {
        [Index(0)]
        public string OldPattern { get; private set; }
        
        [Index(1)]
        public string NewPattern { get; private set; }

        [Index(2), Optional]
        public int? ContentId { get; private set; }
        
        [Index(3)]
        public string RedirectRuleType { get; private set; }
        
        [Index(4)]
        public string RedirectType { get; private set; }
        
        [Index(5), Optional]
        public byte? Priority { get; private set; }

        [Index(6)]
        public string IsActive { get; private set; }
        
        [Index(7)]
        public string Notes { get; private set; }

        public static string[] FieldNames => typeof(RedirectRuleImportRow).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(IndexAttribute)))
            .OrderBy(property => property
                .GetAttributes<IndexAttribute>()
                .First()
                .Index)
            .Select(property => property.Name)
            .ToArray();
        
        public static RedirectRuleImportRow CreateFromRedirectRule(RedirectRule redirectRule)
        {
            return new RedirectRuleImportRow
            {
                OldPattern = redirectRule.OldPattern,
                NewPattern = redirectRule.NewPattern,
                ContentId = redirectRule.ContentId,
                RedirectType = redirectRule.RedirectType.ToString(),
                RedirectRuleType = redirectRule.RedirectRuleType.ToString(),
                Priority = Convert.ToByte(redirectRule.Priority),
                IsActive = redirectRule.IsActive.ToString().ToUpper(),
                Notes = redirectRule.Notes
            };
        }
    }
}
