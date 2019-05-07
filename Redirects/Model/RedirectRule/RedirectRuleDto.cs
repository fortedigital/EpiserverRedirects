using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Data;

namespace Forte.RedirectMiddleware.Model.RedirectRule
{
    public class RedirectRuleDto
    {
        public Identity Id { get; set; }
        
        [Required]
        public string Pattern { get; set; }
        
        [Required]
        public string NewUrl { get; set; }
        
        [Required]
        public RedirectType.RedirectType RedirectType { get; set; }
        
        [Required]
        public DateTimeOffset CreatedOn { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        
        public string CreatedBy { get; set; }
        
        [Required]
        public RedirectRuleType RedirectRuleType { get; set; }

        public RedirectRuleDto()
        {
            
        }
        public RedirectRuleDto(string pattern, string newUrl)
        {
            Pattern = pattern;
            NewUrl = newUrl;
        }
        
        public RedirectRuleDto(Guid guid, string pattern, string newUrl, RedirectType.RedirectType redirectType)
        {
            Id = guid;
            Pattern = pattern;
            NewUrl = newUrl;
        }
    }
}