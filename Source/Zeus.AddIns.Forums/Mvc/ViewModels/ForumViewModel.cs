using Zeus.AddIns.Forums.ContentTypes;
using Zeus.BaseLibrary.Collections.Generic;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class ForumViewModel : BaseForumViewModel<Forum>
	{
		public ForumViewModel(Forum currentItem, IPagination<Topic> topics,
			bool currentUserIsForumAdministrator, string newTopicUrl)
			: base(currentItem)
		{
			Topics = topics;
			CurrentUserIsForumAdministrator = currentUserIsForumAdministrator;
			NewTopicUrl = newTopicUrl;
		}

		public IPagination<Topic> Topics { get; set; }
		public bool CurrentUserIsForumAdministrator { get; set; }
		public bool CurrentUserIsForumModerator { get; set; }
		public string NewTopicUrl { get; set; }
	}
}