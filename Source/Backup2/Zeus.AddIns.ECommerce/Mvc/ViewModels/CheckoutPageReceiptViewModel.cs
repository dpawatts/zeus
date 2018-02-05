using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CheckoutPageReceiptViewModel : ViewModel<CheckoutPage>
	{
		public CheckoutPageReceiptViewModel(CheckoutPage currentItem, string orderNumber, ContentItem contactPage)
			: base(currentItem)
		{
			OrderNumber = orderNumber;
			ContactPage = contactPage;
		}

		public string OrderNumber { get; set; }
		public ContentItem ContactPage { get; set; }
	}
}