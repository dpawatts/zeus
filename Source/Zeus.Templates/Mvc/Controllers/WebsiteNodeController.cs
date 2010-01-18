using System.Web.Mvc;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(WebsiteNode), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class WebsiteNodeController : ZeusController<WebsiteNode>
	{
		public override ActionResult Index()
		{
			return View(new WebsiteNodeViewModel(CurrentItem));
		}
	}
}