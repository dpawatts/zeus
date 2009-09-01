using System;
using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class BlogMonthViewModel : ViewModel<BlogMonth>
	{
		public BlogMonthViewModel(BlogMonth currentItem, DateTime date, IEnumerable<Post> posts)
			: base(currentItem)
		{
			Date = date;
			Posts = posts;
		}

		public DateTime Date { get; set; }
		public IEnumerable<Post> Posts { get; set; }
	}
}