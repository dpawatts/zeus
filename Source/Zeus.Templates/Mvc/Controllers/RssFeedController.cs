using System.Web.Mvc;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(RssFeed), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class RssFeedController : FeedControllerBase<RssFeed>
	{
		public override ActionResult Index()
		{
			return new RssResult(GetFeed());
		}
	}
}