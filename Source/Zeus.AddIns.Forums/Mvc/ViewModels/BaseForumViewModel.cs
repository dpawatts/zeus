using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Services;
using Zeus.Web.Mvc.ViewModels;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class BaseForumViewModel<T> : ViewModel<T>, IBaseForumViewModel
		where T : ContentItem
	{
		public BaseForumViewModel(T currentItem)
			: base(currentItem)
		{
			
		}

		public Member CurrentMember { get; set; }
		public MessageBoard CurrentMessageBoard { get; set; }
		public IForumService ForumService { get; set; }
		public bool LoggedIn { get; set; }
		public string SearchUrl { get; set; }
	}

	public interface IBaseForumViewModel
	{
		Member CurrentMember { get; set; }
		MessageBoard CurrentMessageBoard { get; set; }
		IForumService ForumService { get; set; }
		bool LoggedIn { get; set; }
		string SearchUrl { get; set; }
	}
}