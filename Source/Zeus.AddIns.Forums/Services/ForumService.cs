using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using Zeus.AddIns.Forums.Configuration;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.Security;

namespace Zeus.AddIns.Forums.Services
{
	public class ForumService : IForumService
	{
		private readonly Dictionary<string, string> _badWordReplacements;

		public ForumService()
		{
			_badWordReplacements = new Dictionary<string, string>();

			ForumSection forumConfig = ConfigurationManager.GetSection("zeus.addIns/forums") as ForumSection;
			if (forumConfig != null)
				foreach (BadWordElement badWord in forumConfig.BadWords)
				{
					string replacement = (!string.IsNullOrEmpty(badWord.Replacement)) ? badWord.Replacement : forumConfig.BadWords.DefaultReplacement;
					_badWordReplacements.Add(badWord.Word, replacement);
				}
		}

		public Member GetMember(MessageBoard messageBoard, User user, bool create)
		{
			Member member = null;
			if (user != null)
				member = Find.EnumerateChildren(messageBoard).OfType<Member>().SingleOrDefault(m => m.User == user);

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

		public bool CanEditPost(Post post, Member member)
		{
			if (member == null)
				return false;

			if (post.Author == member)
				return true;

			if (post.Topic.Forum.Moderator != null && post.Topic.Forum.Moderator == member)
				return true;

			return false;
		}

		public Post CreateReply(Topic topic, Member member, string subject, string message)
		{
			// Create post.
			Post post = new Post { Author = member, Title = CleanBadWords(subject), Message = CleanBadWords(message) };
			SetCommonPostProperties(topic, post);
			post.AddTo(topic);

			// Save post.
			Context.Persister.Save(post);

			return post;
		}

		public Topic CreateTopic(Forum forum, Member member, string subject, string message)
		{
			// Create topic first.
			Topic topic = new Topic { Title = CleanBadWords(subject), Author = member };
			topic.AddTo(forum);

			// Then create post.
			Post post = new Post { Author = member, Title = CleanBadWords(subject), Message = CleanBadWords(message) };
			SetCommonPostProperties(topic, post);
			post.AddTo(topic);

			// Save topic, which will also save post.
			Context.Persister.Save(topic);

			return topic;
		}

		private static void SetCommonPostProperties(Topic topic, Post post)
		{
			post.Number = topic.GetChildren<Post>().Max(fi => fi.Number) + 1;
			post.Title = "Post #" + post.Number;
		}

		public Post EditPost(Post post, Member member, string newSubject, string newMessage)
		{
			// Check post is being edited by original author.
			if (post.Author != member)
				throw new ZeusException("Post can only be edited by original author.");

			// Update properties
			if (post.Topic.FirstPost == post)
				post.Topic.Title = CleanBadWords(newSubject);
			post.Message = CleanBadWords(newMessage);

			post.Topic.Updated = DateTime.Now;

			// Save post.
			Context.Persister.Save(post);

			return post;
		}

		private string CleanBadWords(string text)
		{
			if (string.IsNullOrEmpty(text))
				return text;

			foreach (var badWordReplacement in _badWordReplacements)
				text = Regex.Replace(text, Regex.Escape(badWordReplacement.Key), badWordReplacement.Value, RegexOptions.IgnoreCase);

			return text;
		}

		public void Start()
		{
			Context.Persister.ItemSaved += OnItemSaved;
		}

		public void Stop()
		{
			
		}

		protected virtual void OnItemSaved(object sender, ItemEventArgs e)
		{
			
		}
	}
}