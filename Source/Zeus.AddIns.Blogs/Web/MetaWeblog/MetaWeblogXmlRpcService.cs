using System;
using System.Linq;
using CookComputing.XmlRpc;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Services;
using Zeus.AddIns.Blogs.Web.XmlRpc;
using Zeus.FileSystem;
using Zeus.Web.Security;

namespace Zeus.AddIns.Blogs.Web.MetaWeblog
{
	public class MetaWeblogXmlRpcService : ZeusXmlRpcService, IMetaWeblog
	{
		#region IMetaWeblog Members

		string IMetaWeblog.AddPost(string blogid, string username, string password, Post post, bool publish)
		{
			if (ValidateUser(username, password))
			{
				ContentTypes.Post newPost = GetBlogService().AddPost(GetBlog(blogid), post.dateCreated, post.title, post.description, post.categories, publish);
				return newPost.ID.ToString();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		bool IMetaWeblog.UpdatePost(string postid, string username, string password, Post post, bool publish)
		{
			if (ValidateUser(username, password))
			{
				GetBlogService().UpdatePost(GetPost(postid), post.dateCreated, post.title, post.description, post.categories, publish);
				return true;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		Post IMetaWeblog.GetPost(string postid, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				return PopulatePost(GetPost(postid));
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
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

		Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password,
			int numberOfPosts)
		{
			if (ValidateUser(username, password))
			{
				return GetBlogService().GetRecentPosts(GetBlog(blogid), numberOfPosts)
					.Select(p => PopulatePost(p))
					.ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, string username, string password,
			MediaObject mediaObject)
		{
			if (ValidateUser(username, password))
			{
				File file = GetBlogService().AddFile(GetBlog(blogid), mediaObject.name, mediaObject.type, mediaObject.bits);
				return new MediaObjectInfo
				{
					url = file.Url
				};
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
		{
			if (ValidateUser(username, password))
			{
				Zeus.Context.Persister.Delete(GetPost(postid));
				return true;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
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

		UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				//UserInfo info = new UserInfo();

				// TODO: Implement your own logic to get user info objects and set the info
				throw new NotImplementedException();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		#endregion

		#region Private Methods

		private static bool ValidateUser(string username, string password)
		{
			return WebSecurityEngine.Get<ICredentialService>().ValidateUser(username, password);
		}

		private static string GetLink(string url)
		{
			return Zeus.Context.Current.WebContext.GetFullyQualifiedUrl(url);
		}

		private static Blog GetBlog(string id)
		{
			return Zeus.Context.Persister.Get<Blog>(Convert.ToInt32(id));
		}

		private static ContentTypes.Post GetPost(string id)
		{
			return Zeus.Context.Persister.Get<ContentTypes.Post>(Convert.ToInt32(id));
		}

		private static IBlogService GetBlogService()
		{
			return Zeus.Context.Current.Resolve<IBlogService>();
		}

		private static Post PopulatePost(ContentTypes.Post post)
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