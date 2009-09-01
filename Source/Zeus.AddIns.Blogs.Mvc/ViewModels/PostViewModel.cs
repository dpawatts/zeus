using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class PostViewModel : ViewModel<Post>
	{
		public PostViewModel(Post currentItem)
			: base(currentItem)
		{
			
		}
	}
}