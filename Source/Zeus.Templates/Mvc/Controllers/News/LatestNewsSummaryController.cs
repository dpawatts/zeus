using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.Templates.ContentTypes.News;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers.News
{
	[Controls(typeof(LatestNewsSummary), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class LatestNewsSummaryController : ZeusController<LatestNewsSummary>
	{
		public override ActionResult Index()
		{
			var newsItems = (CurrentItem.NewsSection == null) ? new List<NewsItem>() : Find.EnumerateAccessibleChildren(CurrentItem.NewsSection)
				.OfType<NewsItem>().OrderByDescending(ni => ni.Date)
				.Take(CurrentItem.NumberToShow);
			return PartialView(new LatestNewsSummaryViewModel(CurrentItem, newsItems));
		}
	}
}