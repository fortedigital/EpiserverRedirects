using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    [ModelBinder(typeof(RedirectRuleDtoModelBinder))]
    public class RedirectRuleDto
    {
        public Guid? Id { get; set; }
        
        [Required]
        public string OldPattern { get; set; }
        
        [Required]
        public string NewPattern { get; set; }
        
        public int? ContentId { get; set; }
        
        [Required]
        public RedirectRuleType RedirectRuleType { get; set; }
        
        [Required]
        public RedirectType RedirectType { get; set; }
        
        public RedirectOrigin RedirectOrigin { get; set; }
        
        [Required]
        public DateTime CreatedOn { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        
        public string CreatedBy { get; set; }
        
        
        public RedirectRuleDto()
        {
            
        }
        public RedirectRuleDto(string pattern, string newUrl)
        {
            OldPattern = pattern;
            NewPattern = newUrl;
        }
        
        public RedirectRuleDto(Guid guid, string pattern, string newUrl, RedirectType temporary)
        {
            Id = guid;
            OldPattern = pattern;
            NewPattern = newUrl;
        }
    }
}