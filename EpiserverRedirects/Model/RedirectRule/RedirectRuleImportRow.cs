using System;
using System.Linq;
using Castle.Core.Internal;
using CsvHelper.Configuration.Attributes;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    public class RedirectRuleImportRow
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

        public static string[] FieldNames => typeof(RedirectRuleImportRow).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(IndexAttribute)))
            .OrderBy(property => property
                .GetAttributes<IndexAttribute>()
                .First()
                .Index)
            .Select(property => property.Name)
            .ToArray();
    }
}
