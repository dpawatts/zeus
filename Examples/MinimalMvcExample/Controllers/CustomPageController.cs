using System.Linq;
using System.Web.Mvc;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
	[Controls(typeof(CustomPage))]
	public class CustomPageController : ZeusController<CustomPage>
	{
		public override ActionResult Index()
		{
			var something = Engine.Finder.QueryItems<CustomPage>().Where(cp => cp.Content.Contains("blah")) //.OrderBy(cp => cp.Content)
				.ToList();
			return View(CurrentItem);
		}
	}
}