using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class CategoryContainerViewModel : ViewModel<CategoryContainer>
	{
		public IEnumerable<Category> Categories { get; set; }

		public CategoryContainerViewModel(CategoryContainer currentItem, IEnumerable<Category> categories)
			: base(currentItem)
		{
			Categories = categories;
		}
	}
}