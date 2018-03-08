using System.Web.Mvc;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	public class PageNavigationController : Controller
	{
		public ActionResult Navigation(string url)
		{
			PathData pathData = Context.UrlParser.ResolvePath(url);
			var viewData = new PageNavigationViewData
			{
				CurrentPage = pathData.CurrentItem
			};
			return View(viewData);
		}

		public ActionResult Breadcrumbs(string url)
		{
			PathData pathData = Context.UrlParser.ResolvePath(url);
			var viewData = new PageNavigationViewData
			{
				CurrentPage = pathData.CurrentItem
			};
			return View(viewData);
		}
	}

	public class PageNavigationViewData
	{
		public ContentItem CurrentPage;
	}
}