using System;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class BlogMonthViewModel : ViewModel<BlogMonth>
	{
		public BlogMonthViewModel(BlogMonth currentItem, DateTime date, IPageable<Post> posts)
			: base(currentItem)
		{
			Date = date;
			Posts = posts;
		}

		public DateTime Date { get; set; }
		public IPageable<Post> Posts { get; set; }
	}
}