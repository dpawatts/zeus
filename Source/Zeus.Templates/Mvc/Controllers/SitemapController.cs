using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(Sitemap), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class SitemapController : ZeusController<Sitemap>
	{
		public override ActionResult Index()
		{
			return View(new SitemapViewModel(CurrentItem, Find.StartPage));
		}
	}
}