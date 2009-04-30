using System;
using System.Linq;
using Isis.Web.Security;
using Zeus.AddIns.Forums.ContentTypes;

namespace Zeus.AddIns.Forums.Services
{
	public class ForumService : IForumService
	{
		public Member GetMember(MessageBoard messageBoard, IUser user, bool create)
		{
			Member member = Find.EnumerateChildren(messageBoard).OfType<Member>().SingleOrDefault(m => m.User == user);
			if (member == null && user != null && create)
			{
				member = new Member { User = user };
				member.AddTo(messageBoard.GetChildren<MemberContainer>().First());
				Context.Persister.Save(member);
			}
			return member;
		}

		public void ToggleTopicStickiness(Topic topic, Member member)
		{
			// Check that member is the forum moderator.
			if (topic.Forum.Moderator == null || topic.Forum.Moderator != member)
				throw new ZeusException("Topic stickiness can only be changed by forum moderator.");

			topic.Sticky = !topic.Sticky;
			Context.Persister.Save(topic);
		}

		public void TrashTopic(Topic topic, Member member)
		{
			// Check that member is the forum moderator.
			if (topic.Forum.Moderator == null || topic.Forum.Moderator != member)
				throw new ZeusException("Topic can only be trashed by forum moderator.");

			Context.Persister.Delete(topic);
		}

		public Post CreateReply(Topic topic, Member member, string subject, string message)
		{
			// Create post.
			Post post = new Post { Author = member, Title = subject, Message = message };
			post.AddTo(topic);

			// Save post.
			Context.Persister.Save(post);

			return post;
		}

		public Topic CreateTopic(Forum forum, Member member, string subject, string message)
		{
			// Create topic first.
			Topic topic = new Topic { Title = subject, Author = member };
			topic.AddTo(forum);

			// Then create post.
			Post post = new Post { Author = member, Title = subject, Message = message };
			post.AddTo(topic);

			// Save topic, which will also save post.
			Context.Persister.Save(topic);

			return topic;
		}

		public Post EditPost(Post post, Member member, string newSubject, string newMessage)
		{
			// Check post is being edited by original author.
			if (post.Author != member)
				throw new ZeusException("Post can only be edited by original author.");

			// Update properties
			post.Title = newSubject;
			post.Message = newMessage;

			post.Topic.Updated = DateTime.Now;

			// Save post.
			Context.Persister.Save(post);

			return post;
		}

		public void Start()
		{
			Context.Persister.ItemSaved += OnItemSaved;
		}

		public void Stop()
		{
			throw new System.NotImplementedException();
		}

		protected virtual void OnItemSaved(object sender, ItemEventArgs e)
		{
			
		}
	}
}