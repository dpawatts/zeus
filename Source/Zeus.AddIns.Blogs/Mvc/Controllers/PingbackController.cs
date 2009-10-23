using System.Web.Mvc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Mvc.ViewModels;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.Mvc.Controllers
{
	[Controls(typeof(Pingback), AreaName = BlogsWebPackage.AREA_NAME)]
	public class PingbackController : ZeusController<Pingback>
	{
		public override ActionResult Index()
		{
			return PartialView(new PingbackViewModel(CurrentItem));
		}
	}
}