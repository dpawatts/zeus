using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class ProductViewModel : ViewModel<Product>
	{
		public ProductViewModel(Product currentItem)
			: base(currentItem)
		{
			
		}
	}
}