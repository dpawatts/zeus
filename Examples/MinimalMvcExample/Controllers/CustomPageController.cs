using System.Collections.Generic;
using System.Linq;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
	[Controls(typeof(CustomPage))]
	public class CustomPageController : BaseController<CustomPage, ICustomPageViewData>
	{
		public override System.Web.Mvc.ActionResult Index()
		{
			TypedViewData.SearchResults = Context.Finder.Items<CustomPage>()
				.Where(cp => cp.Content == "<p>test</p>")
				//.OrderBy(cp => cp.Content)
				;
			return View("Index", TypedViewData);
		}
	}

	public interface ICustomPageViewData : IViewData<CustomPage>
	{
		IEnumerable<CustomPage> SearchResults { get; set; }
	}
}