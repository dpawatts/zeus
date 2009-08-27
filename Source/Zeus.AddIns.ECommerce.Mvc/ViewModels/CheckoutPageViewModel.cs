using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CheckoutPageViewModel : ViewModel<CheckoutPage>
	{
		public CheckoutPageViewModel(CheckoutPage currentItem)
			: base(currentItem)
		{
			
		}
	}
}