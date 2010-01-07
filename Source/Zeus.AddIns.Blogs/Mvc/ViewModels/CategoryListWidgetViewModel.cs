using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class CategoryListWidgetViewModel : ViewModel<CategoryListWidget>
	{
		public CategoryListWidgetViewModel(CategoryListWidget currentItem, IEnumerable<CategoryListCategoryEntry> categoryEntries)
			: base(currentItem)
		{
			CategoryEntries = categoryEntries;
		}

		public IEnumerable<CategoryListCategoryEntry> CategoryEntries { get; set; }
	}

	public class CategoryListCategoryEntry
	{
		public Category Category { get; set; }
		public int Count { get; set; }
	}
}