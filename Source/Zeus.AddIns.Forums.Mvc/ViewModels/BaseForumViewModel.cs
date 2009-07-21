using Zeus.AddIns.Forums.ContentTypes;
using Zeus.AddIns.Forums.Services;
using Zeus.Web.Mvc;
using Zeus.Web.Mvc.ViewModels;
using Zeus.Web.UI;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class BaseForumViewModel<T> : ViewModel<T>
		where T : ContentItem
	{
		public BaseForumViewModel(T currentItem) : base(currentItem)
		{

		}

		public IForumService ForumService { get; set; }
		public Member CurrentMember { get; set; }
		public string SearchUrl { get; set; }
	}
}