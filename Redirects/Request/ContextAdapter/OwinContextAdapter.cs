using Microsoft.Owin;

namespace Forte.RedirectMiddleware.Request.ContextAdapter
{
    public class OwinContextAdapter : ContextAdapter
    {
        private readonly IOwinContext _owinContext;
        public OwinContextAdapter(IOwinContext owinContext) : base(owinContext.Request.Uri)
        {
            _owinContext = owinContext;
        }
        
        public override void Redirect()
        {
            if (_owinContext == null)
                return;
            
            _owinContext.Response.Headers.Set(LocationHeader, Location);
            _owinContext.Response.StatusCode = StatusCode;
        }
    }
}