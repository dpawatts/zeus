using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(StartPage), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class StartPageController : ZeusController<StartPage>
	{
		public override ActionResult Index()
		{
			return View(new StartPageViewModel(CurrentItem));
		}
	}
}