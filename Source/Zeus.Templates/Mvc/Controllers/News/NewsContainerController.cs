using System.Linq;
using System.Web.Mvc;
using Zeus.Templates.ContentTypes.News;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers.News
{
	[Controls(typeof(NewsContainer), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class NewsContainerController : ZeusController<NewsContainer>
	{
		public override ActionResult Index()
		{
			var newsItems = Find.EnumerateAccessibleChildren(CurrentItem).OfType<NewsItem>().OrderByDescending(ni => ni.Date);
			if (newsItems.Any())
				return Redirect(newsItems.First().Url);
			return Redirect(Find.StartPage.Url);
		}
	}
}