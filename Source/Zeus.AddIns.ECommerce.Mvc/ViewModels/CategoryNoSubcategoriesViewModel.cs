using MvcContrib.Pagination;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CategoryNoSubcategoriesViewModel : ViewModel<Category>
	{
		public CategoryNoSubcategoriesViewModel(Category currentItem, IPagination<Product> products)
			: base(currentItem)
		{
			Products = products;
		}

		public IPagination<Product> Products { get; set; }
	}
}