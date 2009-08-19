using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;
using Zeus.Web;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(Shop), AreaName = ECommerceWebPackage.AREA_NAME)]
	public class ShopController : ZeusController<Shop>
	{
		public override ActionResult Index()
		{
			return View(new ViewModel<Shop>(CurrentItem));
		}
	}
}