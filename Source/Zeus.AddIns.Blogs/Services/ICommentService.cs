using System;
using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;

namespace Zeus.AddIns.Blogs.Services
{
	public interface ICommentService
	{
		Comment AddComment(Post post, string name, string email, string url, string text);
		void AddPingback(Post post, string sourcePageTitle, string sourceUrl);
		void DeleteComment(FeedbackItem feedbackItem);
		void EditComment(FeedbackItem feedbackItem, DateTime created, string name, string email, string url, string text, FeedbackItemStatus status);
		IEnumerable<FeedbackItem> GetDisplayedComments(Post post);
		IEnumerable<FeedbackItem> GetAllComments(Post post);
	}
}