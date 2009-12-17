using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class CategoryViewModel : ViewModel<Category>
	{
		public IEnumerable<Post> Posts { get; set; }

		public CategoryViewModel(Category currentItem, IEnumerable<Post> posts)
			: base(currentItem)
		{
			Posts = posts;
		}
	}
}