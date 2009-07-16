using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.Mvc;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(StartPage), AreaName = "Templates")]
	public class StartPageController : Controller
	{
		public ActionResult Index()
		{
			return View(ControllerContext.RouteData.Values[ContentRoute.ContentItemKey]);
		}
	}
}