using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Forte.Redirects.Model.RedirectRule
{
    public enum RedirectRuleType { ExactMatch, Regex, Wildcard}
    
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
        
        public RedirectRule(string oldPath, string newPattern)
        {
            OldPath = UrlPath.UrlPath.Parse(oldPath);
            NewPattern = newPattern;
        }
        public RedirectRule(Identity id, string oldPath, string newPattern, RedirectType.RedirectType redirectType)
        {
            Id = id;
            OldPath = UrlPath.UrlPath.Parse(oldPath);
            NewPattern = newPattern;
        }
        public Identity Id { get; set; }
        public UrlPath.UrlPath OldPath { get; set; }
        
        public int? ContentId { get; set; }
        public string OldPattern{ get; set; }
        public string NewPattern { get; set; }
        
        public RedirectType.RedirectType RedirectType { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public RedirectRuleType RedirectRuleType { get; set; }
    }

}