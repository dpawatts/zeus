using System.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class ProductViewModel : ViewModel<Product>
	{
		public ProductViewModel(Product currentItem, Category parentCategory,
			IEnumerable<Subcategory> parentCategorySubcategories)
			: base(currentItem)
		{
			ParentCategory = parentCategory;
			ParentCategorySubcategories = parentCategorySubcategories;
		}

		public Category ParentCategory { get; set; }
		public IEnumerable<Subcategory> ParentCategorySubcategories { get; set; }
	}
}