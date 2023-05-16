using EPiServer.Data;
using EPiServer.Data.Dynamic;
using System;

// In order to have backward compability (with DDS) we have to preserve both namespace and class name.
namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class RedirectRule : IDynamicData, IRedirectRule
    {
        public Identity Id { get; set; }

        public Guid RuleId => this.Id.ExternalId;
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
        public Guid? HostId { get; set; }
    }
}
