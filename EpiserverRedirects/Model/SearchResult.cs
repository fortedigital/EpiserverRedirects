using System.Collections.Generic;

namespace Forte.EpiserverRedirects.Model
{
    public class SearchResult<T>
    {
        public IList<T> Items { get; set; }

        public int Total { get; set; }
    }
}
