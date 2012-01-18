using System.Web.Mvc;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Examples.MinimalMvcExample.ViewModels;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.Controllers;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ViewModels;
using Zeus.AddIns.ECommerce.PaypalExpress;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
    [Controls(typeof(BasketPage), AreaName = WebsiteAreaRegistration.AREA_NAME)]
    public class BasketPageController : PayPalBasketPageController<BasketPage>
    {
        public override ActionResult Index()
        {
            return View(new BasketPageViewModel(CurrentItem));
        }

        public override IPayPalBasketPageViewModel GetViewModel(BasketPage CurrentItem)
        {
            return new BasketPageViewModel(CurrentItem);
        }

        public override void PayPalOrderSuccess(IPayPalBasketPageViewModel viewModel, Address shippingAddress, string token)
        {
            //this is where you process the order!!
        }
    }
}
