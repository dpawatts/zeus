using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Web.Mvc.ViewModels;
using System.Collections.Generic;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class BlogViewModel : ViewModel<Blog>
	{
        public BlogViewModel(Blog currentItem, IPageable<Post> recentPosts, IEnumerable<Post> allPosts)
			: base(currentItem)
		{
            AllPosts = allPosts;
			RecentPosts = recentPosts;
		}

		public IPageable<Post> RecentPosts { get; set; }
        public IEnumerable<Post> AllPosts { get; set; }
	}
}