using System.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class CategoryViewModel : ViewModel<Category>
	{
		public CategoryViewModel(Category currentItem, IEnumerable<Product> products)
			: base(currentItem)
		{
			Products = products;
		}

		public IEnumerable<Product> Products { get; set; }
	}
}