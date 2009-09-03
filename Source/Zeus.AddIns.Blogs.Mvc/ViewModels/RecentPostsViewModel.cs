using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class RecentPostsViewModel : ViewModel<RecentPosts>
	{
		public RecentPostsViewModel(RecentPosts currentItem, IEnumerable<Post> recentPosts)
			: base(currentItem)
		{
			RecentPosts = recentPosts;
		}

		public IEnumerable<Post> RecentPosts { get; set; }
	}
}