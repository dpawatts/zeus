using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class BlogYearViewModel : ViewModel<BlogYear>
	{
		public BlogYearViewModel(BlogYear currentItem, int year, IPageable<Post> posts)
			: base(currentItem)
		{
			Year = year;
			Posts = posts;
		}

		public int Year { get; set; }
		public IPageable<Post> Posts { get; set; }
	}
}