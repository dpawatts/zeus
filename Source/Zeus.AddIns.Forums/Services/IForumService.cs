using Castle.Core;
using Isis.ComponentModel;
using Isis.Web.Security;
using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Services
{
	public interface IForumService : IService, IStartable
	{
		bool CanEditPost(Post post, Member member);
		Post CreateReply(Topic topic, Member member, string subject, string message);
		Topic CreateTopic(Forum forum, Member member, string subject, string message);
		Post EditPost(Post post, Member member, string newSubject, string newMessage);
		Member GetMember(MessageBoard messageBoard, IUser user, bool create);
		string GetPostPreview(string message);
		void ToggleTopicStickiness(Topic topic, Member member);
		void TrashTopic(Topic topic, Member member);
	}
}