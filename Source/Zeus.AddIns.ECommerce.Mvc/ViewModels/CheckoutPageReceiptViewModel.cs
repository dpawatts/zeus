using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CheckoutPageReceiptViewModel : ViewModel<CheckoutPage>
	{
		public CheckoutPageReceiptViewModel(CheckoutPage currentItem)
			: base(currentItem)
		{
			
		}
	}
}