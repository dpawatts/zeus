using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Mvc.ViewModels
{
	public class MemberViewModel : BaseForumViewModel<Member>
	{
		public MemberViewModel(Member currentItem)
			: base(currentItem)
		{
		}
	}
}