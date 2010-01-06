using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class BlogViewModel : ViewModel<Blog>
	{
		public BlogViewModel(Blog currentItem, IPageable<Post> recentPosts)
			: base(currentItem)
		{
			RecentPosts = recentPosts;
		}

		public IPageable<Post> RecentPosts { get; set; }
	}
}