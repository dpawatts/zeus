using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.Mvc;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(StartPage), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class StartPageController : ZeusController<StartPage>
	{
		public override ActionResult Index()
		{
			return View(new ViewModel<StartPage>(CurrentItem));
		}
	}
}