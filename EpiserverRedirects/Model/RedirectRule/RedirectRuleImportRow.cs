using CsvHelper.Configuration.Attributes;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    public class RedirectRuleImportRow
    {
        [Index(0)]
        public string OldPattern { get; set; }
        
        [Index(1)]
        public string NewPattern { get; set; }

        [Index(2)]
        public string RedirectType { get; set; }
        
        [Index(3)]
        public string RedirectRuleType { get; set; }
        
        [Index(4)]
        public string IsActive { get; set; }
        
        [Index(5)]
        public string Notes { get; set; }
    }
}