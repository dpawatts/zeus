using System.Web.Mvc;
using Zeus.Templates.ContentTypes.Widgets;
using Zeus.Templates.Mvc.ViewModels;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(TextItem), AreaName = TemplatesWebPackage.AREA_NAME)]
	public class TextItemController : WidgetController<TextItem>
	{
		public override ActionResult Index()
		{
			return PartialView(new TextItemViewModel(CurrentItem));
		}
	}
}