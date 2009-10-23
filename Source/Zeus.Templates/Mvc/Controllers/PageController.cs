using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(Page), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class PageController : ZeusController<Page>
	{
		public override ActionResult Index()
		{
			return View(new PageViewModel(CurrentItem));
		}
	}
}