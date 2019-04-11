using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Forte.RedirectMiddleware.Model
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class RedirectModel : IDynamicData
    {
        public RedirectModel()
        {
            
        }
        public RedirectModel(Identity id)
        {
            Id = id;
        }
        
        public RedirectModel(string oldPath, string newUrl)
        {
            OldPath = oldPath;
            NewUrl = newUrl;
        }
        public RedirectModel(Identity id, string oldPath, string newUrl, StatusCode statusCode)
        {
            Id = id;
            OldPath = oldPath;
            NewUrl = newUrl;
        }
        public Identity Id { get; set; }
        public string OldPath { get; set; }
        public string NewUrl { get; set; }
        
        public StatusCode StatusCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }

        public bool Validate()
        {
            try
            {
                var trimmedOldPath = OldPath.Trim();
                var oldPathUri = new Uri(trimmedOldPath, UriKind.Relative);

                OldPath = oldPathUri.OriginalString;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public static string NormalizePath(string path)
        {
            path = path[0] == '/'
                ? path
                : '/' + path;

            if (path.Length > 1)
                path = path.TrimEnd('/');

            return path;
        }

    }

    public enum StatusCode { MovedPermanently = 301, Found = 302} 
}