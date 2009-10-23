using System.Web.Mvc;
using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(FeedLink), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class FeedLinkController : WidgetController<FeedLink>
	{
		public override ActionResult Index()
		{
			return PartialView(new FeedLinkViewModel(CurrentItem));
		}

		public override ActionResult Header()
		{
			return PartialView(new FeedLinkViewModel(CurrentItem));
		}
	}
}