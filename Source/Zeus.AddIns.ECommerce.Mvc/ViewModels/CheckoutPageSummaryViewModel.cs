using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CheckoutPageSummaryViewModel : ViewModel<CheckoutPage>
	{
		public CheckoutPageSummaryViewModel(CheckoutPage currentItem)
			: base(currentItem)
		{
			
		}
	}
}