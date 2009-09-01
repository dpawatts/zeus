using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class BlogYearViewModel : ViewModel<BlogYear>
	{
		public BlogYearViewModel(BlogYear currentItem, int year, IEnumerable<Post> posts)
			: base(currentItem)
		{
			Year = year;
			Posts = posts;
		}

		public int Year { get; set; }
		public IEnumerable<Post> Posts { get; set; }
	}
}