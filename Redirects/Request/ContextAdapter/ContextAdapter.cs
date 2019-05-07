using System;

namespace Forte.RedirectMiddleware.Request.ContextAdapter
{
    public abstract class ContextAdapter
    {
        protected const string LocationHeader = "Location";

        protected ContextAdapter(Uri oldUri)
        {
            OldUri = oldUri;
        }

        public Uri OldUri { get; }
        public int StatusCode { protected get; set; }
        public string Location { protected get; set; }

        public abstract void Redirect();
    }
}