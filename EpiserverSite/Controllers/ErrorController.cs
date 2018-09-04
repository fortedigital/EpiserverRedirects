using System.Web.Mvc;
using EpiserverSite.UrlRewritePlugin;
using static System.String;

namespace EpiserverSite.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/");
        }

        public ActionResult NotFound(string aspxerrorpath)
        {
            var url = aspxerrorpath ?? (RouteData.Values["Url"]?.ToString() ?? Empty);
            url = url.NormalizePath();
            
            
            if (IsNullOrEmpty(url) )
                return Redirect("/");

            var urlReqriteModel = RedirectHelper.GetRedirectModel(url);

            return Redirect(urlReqriteModel == null ? "/" : urlReqriteModel.NewUrl);
        }
    }
}