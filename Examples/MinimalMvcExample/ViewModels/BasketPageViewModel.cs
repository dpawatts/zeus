using Zeus.Web.Mvc.ViewModels;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ViewModels;

namespace Zeus.Examples.MinimalMvcExample.ViewModels
{
    public class BasketPageViewModel : PayPalBasketPageViewModel<BasketPage>
    {
        public BasketPageViewModel(BasketPage currentItem)
            : base(currentItem)
        {

        }
    }
}
