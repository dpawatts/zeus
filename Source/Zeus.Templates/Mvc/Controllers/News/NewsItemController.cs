using System.Linq;
using System.Web.Mvc;
using Zeus.Templates.ContentTypes.News;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers.News
{
	[Controls(typeof(NewsItem), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class NewsItemController : ZeusController<NewsItem>
	{
		public override ActionResult Index()
		{
			var allNewsItems = Find.EnumerateAccessibleChildren(CurrentItem.CurrentNewsContainer)
				.OfType<NewsItem>().OrderByDescending(ni => ni.Date);
			return View(new NewsItemViewModel(CurrentItem, allNewsItems));
		}
	}
}