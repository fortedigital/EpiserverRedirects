using System.Collections.Generic;

namespace Redirects
{
    public interface IRepository
    {
        string FindNewPath(string oldPath);
    }
    
    public abstract class Repository : IRepository
    {
        public abstract string FindNewPath(string oldPath);
    }

    public class DictionaryRepository : Repository
    {
        private readonly Dictionary<string, string> _redirectsDictionary;

        public DictionaryRepository(Dictionary<string, string> redirectsDictionary)
        {
            _redirectsDictionary = redirectsDictionary;
        }

        public override string FindNewPath(string oldPath)
        {
            _redirectsDictionary.TryGetValue(oldPath, out var newPath);
            return newPath;
        }
    }
    
    public class DynamicDataStoreRepository : Repository
    {
        public override string FindNewPath(string oldPath)
        {
            throw new System.NotImplementedException();
        }
    }
}