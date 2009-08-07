using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(Sitemap), AreaName = "Templates")]
	public class SitemapController : ZeusController<Sitemap>
	{
		public override ActionResult Index()
		{
			return View(CurrentItem);
		}
	}
}