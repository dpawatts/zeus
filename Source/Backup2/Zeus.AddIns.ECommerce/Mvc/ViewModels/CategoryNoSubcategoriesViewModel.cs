using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CategoryNoSubcategoriesViewModel : ViewModel<Category>
	{
		public CategoryNoSubcategoriesViewModel(Category currentItem, IPageable<Product> products)
			: base(currentItem)
		{
			Products = products;
		}

		public IPageable<Product> Products { get; set; }
	}
}