using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Redirects
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
        
        public RedirectModel(string oldPath, string newPath)
        {
            OldPath = oldPath;
            NewPath = newPath;
        }
        public RedirectModel(Identity id, string oldPath, string newPath)
        {
            Id = id;
            OldPath = oldPath;
            NewPath = newPath;
        }
        public Identity Id { get; set; }
        public string OldPath { get; set; }
        public string NewPath { get; set; }
    }
}