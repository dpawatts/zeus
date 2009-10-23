using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class MessageBoardPostViewModel : BaseForumViewModel<MessageBoard>
	{
		public MessageBoardPostViewModel(MessageBoard currentItem)
			: base(currentItem)
		{

		}

		public Topic CurrentTopic { get; set; }
		public bool TopicSummaryVisible { get; set; }
		public string PreviewMessage { get; set; }
		public bool PreviewVisible { get; set; }

		public string Subject { get; set; }
		public string Message { get; set; }
	}
}