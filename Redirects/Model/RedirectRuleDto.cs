using System;
using EPiServer.Data;

namespace Forte.RedirectMiddleware.Model
{
    public class RedirectRuleDto
    {
        public Identity Id { get; set; }
        public string OldPath { get; set; }
        public string NewUrl { get; set; }
        
        public RedirectType.RedirectType RedirectType { get; set; }
        public DateTime CreatedOn { get; set; }
        
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