using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(WebsiteNode), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class StartPageController : ZeusController<WebsiteNode>
	{
		public override ActionResult Index()
		{
			return View(new StartPageViewModel(CurrentItem));
		}
	}
}