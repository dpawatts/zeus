using Zeus.AddIns.Forums.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class ForumViewModel : BaseForumViewModel<Forum>
	{
		public ForumViewModel(Forum currentItem)
			: base(currentItem)
		{

		}

		public IPagination<Topic> Topics { get; set; }
		public bool CurrentUserIsForumModerator { get; set; }
	}
}