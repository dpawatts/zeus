using System.Collections.Generic;
using Isis.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Web.Mvc.ViewModels;
using MvcContrib.Pagination;

namespace Zeus.AddIns.ECommerce.Mvc.ViewModels
{
	public class SubcategoryViewModel : ViewModel<Subcategory>
	{
		public SubcategoryViewModel(Subcategory currentItem, Category parentCategory,
			IEnumerable<Subcategory> parentCategorySubcategories,
			IPageable<Product> products)
			: base(currentItem)
		{
			ParentCategory = parentCategory;
			ParentCategorySubcategories = parentCategorySubcategories;
			Products = products;
		}

		public Category ParentCategory { get; set; }
		public IEnumerable<Subcategory> ParentCategorySubcategories { get; set; }
		public IPageable<Product> Products { get; set; }
	}
}