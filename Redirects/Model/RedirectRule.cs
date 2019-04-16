using System;
using System.Net;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Forte.RedirectMiddleware.Model
{
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
        
        public RedirectRule(string oldPath, string newUrl)
        {
            OldPath = UrlPath.Create(oldPath);
            NewUrl = newUrl;
        }
        public RedirectRule(Identity id, string oldPath, string newUrl, RedirectType.RedirectType redirectType)
        {
            Id = id;
            OldPath = UrlPath.Create(oldPath);
            NewUrl = newUrl;
        }
        public Identity Id { get; set; }
        public UrlPath OldPath { get; set; }
        public string NewUrl { get; set; }
        
        public RedirectType.RedirectType RedirectType { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}