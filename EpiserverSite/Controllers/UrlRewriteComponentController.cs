using System.Web.Mvc;
using EPiServer.Shell.ViewComposition;

namespace EpiserverSite.Controllers
{
//    [Authorize(Roles = "WebEditors, WedAdmins, Administrators")]
    [IFrameComponent(Url = "UrlRewriteComponent")]
    public class UrlRewriteComponentController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return Content("<div>AAAA</div>");
        }
    }
}