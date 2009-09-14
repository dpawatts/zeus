using System;
using System.Collections.Generic;
using System.Linq;
using CookComputing.XmlRpc;
using Isis.Web;
using Isis.Web.Security;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Services;

namespace Zeus.AddIns.Blogs.Web.MetaWeblog
{
	public class MetaWeblog : XmlRpcService, IMetaWeblog
	{
		#region IMetaWeblog Members

		string IMetaWeblog.AddPost(string blogid, string username, string password,
			Post post, bool publish)
		{
			if (ValidateUser(username, password))
			{
				ContentTypes.Post newPost = GetBlogService().AddPost(GetBlog(blogid), post.dateCreated, post.title, post.description, publish);
				return newPost.ID.ToString();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		bool IMetaWeblog.UpdatePost(string postid, string username, string password,
			Post post, bool publish)
		{
			if (ValidateUser(username, password))
			{
				bool result = false;

				// TODO: Implement your own logic to add the post and set the result
				throw new NotImplementedException();

				return result;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		Post IMetaWeblog.GetPost(string postid, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				return PopulatePost(Zeus.Context.Persister.Get<ContentTypes.Post>(Convert.ToInt32(postid)));
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

				// TODO: Implement your own logic to get category info and set the categoryInfos
				throw new NotImplementedException();

				return categoryInfos.ToArray();
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
				MediaObjectInfo objectInfo = new MediaObjectInfo();

				// TODO: Implement your own logic to add media object and set the objectInfo
				throw new NotImplementedException();

				return objectInfo;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
		{
			if (ValidateUser(username, password))
			{
				bool result = false;

				// TODO: Implement your own logic to delete the post and set the result
				throw new NotImplementedException();

				return result;
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
				UserInfo info = new UserInfo();

				// TODO: Implement your own logic to get user info objects and set the info
				throw new NotImplementedException();

				return info;
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		#endregion

		#region Private Methods

		private static bool ValidateUser(string username, string password)
		{
			return WebSecurityEngine.Get<ICredentialContextService>().GetCurrentService().ValidateUser(username, password);
		}

		private static string GetLink(string url)
		{
			return Zeus.Context.Current.WebContext.Url.HostUrl + url;
		}

		private static Blog GetBlog(string id)
		{
			return Zeus.Context.Persister.Get<Blog>(Convert.ToInt32(id));
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
				postid = post.ID
			};
		}

		#endregion
	}
}