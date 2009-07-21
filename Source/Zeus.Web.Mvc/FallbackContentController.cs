using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	[Controls(typeof(ContentItem))]
	public class FallbackContentController : ContentController<ContentItem>
	{
		public override ActionResult Index()
		{
			ContentItem item = CurrentItem;
			return View(item.FindPath(PathData.DefaultAction).TemplateUrl, item);
		}
	}
}