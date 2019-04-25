using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Data;

namespace Forte.RedirectMiddleware.Model.RedirectRule
{
    public class RedirectRuleDto
    {
        public Identity Id { get; set; }
        
        [Required]
        public string OldPath { get; set; }
        
        [Required]
        public string NewUrl { get; set; }
        
        [Required]
        public RedirectType.RedirectType RedirectType { get; set; }
        
        [Required]
        public DateTime CreatedOn { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
        public string Notes { get; set; }

        public RedirectRuleDto()
        {
            
        }
        public RedirectRuleDto(string oldPath, string newUrl)
        {
            OldPath = oldPath;
            NewUrl = newUrl;
        }
        
        public RedirectRuleDto(Guid guid, string oldPath, string newUrl, RedirectType.RedirectType redirectType)
        {
            Id = guid;
            OldPath = oldPath;
            NewUrl = newUrl;
        }
    }
}