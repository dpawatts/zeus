using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CheckoutPageFailedViewModel : ViewModel<CheckoutPage>
	{
		public CheckoutPageFailedViewModel(CheckoutPage currentItem, string errorMessage, ContentItem contactPage)
			: base(currentItem)
		{
			ErrorMessage = errorMessage;
			ContactPage = contactPage;
		}

		public string ErrorMessage { get; set; }
		public ContentItem ContactPage { get; set; }
	}
}