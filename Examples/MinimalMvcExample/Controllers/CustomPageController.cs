using System.Collections.Generic;
using System.Linq;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
	[Controls(typeof(CustomPage))]
	public class CustomPageController : ZeusController<CustomPage>
	{
		public override System.Web.Mvc.ActionResult Index()
		{
			return View(new CustomPageViewModel(CurrentItem)
			{
				SearchResults =
					Context.Finder.Items<CustomPage>()
						.Where(cp => cp.Content == "<p>test</p>")
				//.OrderBy(cp => cp.Content)
			});
		}
	}

	public class CustomPageViewModel : ViewModel<CustomPage>
	{
		public CustomPageViewModel(CustomPage currentItem)
			: base(currentItem)
		{

		}

		public IEnumerable<CustomPage> SearchResults { get; set; }
	}
}