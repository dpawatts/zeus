using System.Collections.Generic;
using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class MessageBoardViewModel : BaseForumViewModel<MessageBoard>
	{
		public MessageBoardViewModel(MessageBoard currentItem, IEnumerable<Forum> forums)
			: base(currentItem)
		{
			Forums = forums;
		}

		public IEnumerable<Forum> Forums { get; set; }
	}
}