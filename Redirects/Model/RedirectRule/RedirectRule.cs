using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Forte.Redirects.Model.RedirectRule
{
    public enum RedirectRuleType { ExactMatch = 1, Regex = 2, Wildcard = 3}
    
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class RedirectRule : IDynamicData
    {
        public RedirectRule()
        {
            
        }
        public RedirectRule(Identity id)
        {
            Id = id;
        }
        
        public RedirectRule(string oldPattern, string newPattern)
        {
            OldPattern = oldPattern;
            NewPattern = newPattern;
        }
        public RedirectRule(Identity id, string oldPattern, string newPattern, RedirectType.RedirectType redirectType)
        {
            Id = id;
            OldPattern = oldPattern;
            NewPattern = newPattern;
        }
        public Identity Id { get; set; }

        public int? ContentId { get; set; }
        public string OldPattern{ get; set; }
        public string NewPattern { get; set; }

        public RedirectRuleType RedirectRuleType { get; set; }
        
        public RedirectType.RedirectType RedirectType { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }

}