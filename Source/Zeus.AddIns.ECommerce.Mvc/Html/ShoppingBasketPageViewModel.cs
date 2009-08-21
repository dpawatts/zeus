using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class ShoppingBasketPageViewModel : ViewModel<ShoppingBasketPage>
	{
		public ShoppingBasketPageViewModel(ShoppingBasketPage currentItem)
			: base(currentItem)
		{

		}
	}
}