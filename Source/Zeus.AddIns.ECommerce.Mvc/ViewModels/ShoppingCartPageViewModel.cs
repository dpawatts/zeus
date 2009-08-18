using Zeus.AddIns.ECommerce.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
    public class ShoppingCartPageViewModel : ViewModel<ShoppingCartPage>
    {
        public ShoppingCartPageViewModel(ShoppingCartPage currentItem)
            : base(currentItem)
        {

        }
    }
}