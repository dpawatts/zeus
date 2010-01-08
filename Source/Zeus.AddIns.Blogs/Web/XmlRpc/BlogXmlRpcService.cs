using System;
using System.Linq;
using CookComputing.XmlRpc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Services;
using Zeus.FileSystem;
using Zeus.Persistence;
using Zeus.Templates.Services;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.AddIns.Blogs.Web.XmlRpc
{
	public class BlogXmlRpcService : ZeusXmlRpcService, IMetaWeblogApi, IBloggerApi, IWordPressApi
	{
		#region Fields

		private readonly IPersister _persister;
		private readonly IWebContext _webContext;
		private readonly ICredentialService _credentialService;
		private readonly IBlogService _blogService;
		private readonly ICommentService _commentService;
		private readonly ITagService _tagService;

		#endregion

		#region Constructor

		public BlogXmlRpcService(IPersister persister, IWebContext webContext, ICredentialService credentialService,
			IBlogService blogService, ICommentService commentService, ITagService tagService)
		{
			_persister = persister;
			_webContext = webContext;
			_credentialService = credentialService;
			_blogService = blogService;
			_commentService = commentService;
			_tagService = tagService;
		}

		#endregion

		#region MetaWeblog API

		public string NewPost(string blogid, string username, string password, Post post, bool publish)
		{
			if (ValidateUser(username, password))
			{
				ContentTypes.Post newPost = _blogService.AddPost(GetBlog(blogid), post.dateCreated, post.title, post.description, post.categories, publish);
				return newPost.ID.ToString();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public bool EditPost(string postid, string username, string password, Post post, bool publish)
		{
			if (ValidateUser(username, password))
			{
				_blogService.UpdatePost(GetPost(postid), post.dateCreated, post.title, post.description, post.categories, publish);
				return true;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public Post GetPost(string postid, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				return PopulatePost(GetPost(postid));
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public CategoryInfo[] GetCategories(string blogid, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				return GetBlog(blogid).Categories.GetChildren<ContentTypes.Category>()
					.Select(c => new CategoryInfo
						{
							description = c.Title,
							title = c.Title,
							htmlUrl = GetLink(c.Url),
							rssUrl = string.Empty,
							categoryid = c.ID.ToString()
						})
					.ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public Post[] GetRecentPosts(string blogid, string username, string password,
			int numberOfPosts)
		{
			if (ValidateUser(username, password))
			{
				return _blogService.GetRecentPosts(GetBlog(blogid), numberOfPosts)
					.Select(p => PopulatePost(p))
					.ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public MediaObjectInfo NewMediaObject(string blogid, string username, string password,
			MediaObject mediaObject)
		{
			if (ValidateUser(username, password))
			{
				File file = _blogService.AddFile(GetBlog(blogid), mediaObject.name, mediaObject.type, mediaObject.bits);
				return new MediaObjectInfo
					{
						url = file.Url
					};
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		#endregion

		#region Blogger API

		public BlogInfo[] GetUsersBlogs(string key, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				return Find.EnumerateAccessibleChildren(Find.StartPage).OfType<Blog>()
					.Select(b => new BlogInfo
						{
							blogid = b.ID.ToString(),
							blogName = b.Title,
							url = GetLink(b.Url)
						})
					.ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public UserInfo GetUserInfo(string key, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				//UserInfo info = new UserInfo();

				// TODO: Implement your own logic to get user info objects and set the info
				throw new NotImplementedException();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public bool DeletePost(string key, string postid, string username, string password, bool publish)
		{
			if (ValidateUser(username, password))
			{
				Zeus.Context.Persister.Delete(GetPost(postid));
				return true;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		#endregion

		#region WordPress API

		public WordPressBlogInfo[] GetUsersBlogs(string username, string password)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			return Find.EnumerateAccessibleChildren(Find.StartPage).OfType<Blog>()
				.Select(b => new WordPressBlogInfo
					{
						blog_id = b.ID,
						blog_name = b.Title,
						url = GetLink(b.Url),
						is_admin = true
					})
				.ToArray();
		}

		public WordPressTag[] GetTags(int blog_id, string username, string password)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			return _tagService.GetActiveTags(_tagService.GetCurrentTagGroup(GetBlog(blog_id.ToString())))
				.Select(t => new WordPressTag
				{
					count = _tagService.GetReferenceCount(t),
					html_url = GetLink(t.Url),
					name = t.Title,
					slug = t.Name,
					tag_id = t.ID
				})
				.ToArray();
		}

		public WordPressCommentCount[] GetCommentCount(int blog_id, string username, string password, string post_id)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			var comments = _commentService.GetAllComments(GetPost(post_id));
			return new[]
				{
					new WordPressCommentCount
						{
							approved = comments.Count(fi => fi.Status == FeedbackItemStatus.Approved),
							awaiting_moderation = comments.Count(fi => fi.Status == FeedbackItemStatus.Pending),
							spam = comments.Count(fi => fi.Status == FeedbackItemStatus.Spam),
							total_comments = comments.Count()
						}
				};
		}

		public string[] GetPostStatusList(int blog_id, string username, string password)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			return new[]
				{
					"draft",
					"pending",
					"private",
					"publish"
				};
		}

		public string[] GetPageStatusList(int blog_id, string username, string password)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			return new[]
				{
					"draft",
					"private",
					"publish"
				};
		}

		public WordPressPageTemplate[] GetPageTemplates(int blog_id, string username, string password)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			return new[]
				{
					new WordPressPageTemplate { name = "Default", description = "Default Template" }
				};
		}

		public WordPressOption[] GetOptions(int blog_id, string username, string password, string[] options)
		{
			throw new NotImplementedException();
		}

		public WordPressOption[] SetOptions(int blog_id, string username, string password, WordPressOption[] options)
		{
			throw new NotImplementedException();
		}

		public bool DeleteComment(int blog_id, string username, string password, int comment_id)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			FeedbackItem feedbackItem = _persister.Get<FeedbackItem>(comment_id);
			if (feedbackItem == null)
				return false;

			_commentService.DeleteComment(feedbackItem);
			return true;
		}

		public bool EditComment(int blog_id, string username, string password, int comment_id, WordPressEditComment comment)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			FeedbackItem feedbackItem = _persister.Get<FeedbackItem>(comment_id);
			if (feedbackItem == null)
				return false;

			FeedbackItemStatus status;
			switch (comment.status)
			{
				case "hold" :
					status = FeedbackItemStatus.Pending;
					break;
				case "approve" :
					status = FeedbackItemStatus.Approved;
					break;
				case "spam" :
					status = FeedbackItemStatus.Spam;
					break;
				default :
					throw new NotSupportedException("Comment status not supported: " + comment.status);
			}
			_commentService.EditComment(feedbackItem, comment.date_created_gmt, comment.author,
				comment.author_email, comment.author_url, comment.content, status);
			return true;
		}

		public int NewComment(int blog_id, string username, string password, int post_id, WordPressNewComment comment)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			Comment newComment = _commentService.AddComment(GetPost(post_id.ToString()), comment.author, comment.author_email,
				comment.author_url, comment.content);
			return newComment.ID;
		}

		public string[] GetCommentStatusList(int blog_id, string username, string password)
		{
			if (!ValidateUser(username, password))
				throw new XmlRpcFaultException(0, "User is not valid!");

			return new[]
				{
					"hold",
					"approve",
					"spam"
				};
		}

		public WordPressPageInfo GetPage(string blog_id, string page_id, string username, string password)
		{
			throw new NotImplementedException();
		}

		public WordPressPageInfo[] GetPages(string blog_id, string username, string password)
		{
			throw new NotImplementedException();
		}

		public WordPressPage[] GetPageList(string blog_id, string username, string password)
		{
			throw new NotImplementedException();
		}

		public int NewPage(string blog_id, string username, string password, Post content, bool publish)
		{
			throw new NotImplementedException();
		}

		public bool DeletePage(string blog_id, string username, string password, string page_id)
		{
			throw new NotImplementedException();
		}

		public bool EditPage(string blog_id, string page_id, string username, string password, Post content, bool publish)
		{
			throw new NotImplementedException();
		}

		public WordPressAuthor[] GetAuthors(string blog_id, string username, string password)
		{
			throw new NotImplementedException();
		}

		WordPressCategoryInfo[] IWordPressApi.GetCategories(string blog_id, string username, string password)
		{
			throw new NotImplementedException();
		}

		public int NewCategory(string blog_id, string username, string password, WordPressCategory category)
		{
			throw new NotImplementedException();
		}

		public bool DeleteCategory(string blog_id, string username, string password, int category_id)
		{
			throw new NotImplementedException();
		}

		public WordPressSuggestCategory[] SuggestCategories(string blog_id, string username, string password, string category, int max_results)
		{
			throw new NotImplementedException();
		}

		public MediaObjectInfo UploadFile(string blog_id, string username, string password, MediaObject data)
		{
			throw new NotImplementedException();
		}

		public WordPressCommentInfo GetComment(string blog_id, string username, string password, int comment_id)
		{
			throw new NotImplementedException();
		}

		public WordPressCommentInfo[] GetComments(string blog_id, string username, string password, WordPressCommentRequest options)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Private Methods

		private bool ValidateUser(string username, string password)
		{
			return _credentialService.ValidateUser(username, password);
		}

		private string GetLink(string url)
		{
			return _webContext.GetFullyQualifiedUrl(url);
		}

		private Blog GetBlog(string id)
		{
			return _persister.Get<Blog>(Convert.ToInt32(id));
		}

		private ContentTypes.Post GetPost(string id)
		{
			return _persister.Get<ContentTypes.Post>(Convert.ToInt32(id));
		}

		private Post PopulatePost(ContentTypes.Post post)
		{
			return new Post
				{
					dateCreated = post.Created,
					description = post.Text,
					title = post.Title,
					permalink = GetLink(post.Url),
					postid = post.ID,
					categories = post.Categories.Cast<ContentTypes.Category>().Select(c => c.Title).ToArray()
				};
		}

		#endregion
	}
}