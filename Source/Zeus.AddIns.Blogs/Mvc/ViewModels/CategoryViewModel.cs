using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class CategoryViewModel : ViewModel<Category>
	{
		public IPageable<Post> Posts { get; set; }

		public CategoryViewModel(Category currentItem, IPageable<Post> posts)
			: base(currentItem)
		{
			Posts = posts;
		}
	}
}