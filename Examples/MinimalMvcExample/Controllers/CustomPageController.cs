using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
	[Controls(typeof(CustomPage))]
	public class CustomPageController : ZeusController<CustomPage>
	{
		public override ActionResult Index()
		{
			return View(CurrentItem);
		}
	}
}