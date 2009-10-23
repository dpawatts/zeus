using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class CommentViewModel : ViewModel<Comment>
	{
		public CommentViewModel(Comment currentItem)
			: base(currentItem)
		{
			
		}
	}
}