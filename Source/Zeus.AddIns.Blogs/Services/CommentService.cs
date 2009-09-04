using System;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.ContentTypes;
using Zeus.Persistence;

namespace Zeus.AddIns.Blogs.Services
{
	public class CommentService : ICommentService
	{
		private readonly IContentTypeManager _contentTypeManager;
		private readonly IPersister _persister;

		public CommentService(IContentTypeManager contentTypeManager, IPersister persister)
		{
			_contentTypeManager = contentTypeManager;
			_persister = persister;
		}

		public void AddComment(Post post, string name, string url, string text)
		{
			Comment comment = _contentTypeManager.CreateInstance<Comment>(post);
			comment.AuthorName = name;
			comment.AuthorUrl = url;
			comment.Text = text;
			comment.AddTo(post);

			_persister.Save(comment);
		}
	}
}