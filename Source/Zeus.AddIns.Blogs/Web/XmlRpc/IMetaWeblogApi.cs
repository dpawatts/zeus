using CookComputing.XmlRpc;

namespace Zeus.AddIns.Blogs.Web.XmlRpc
{
	public interface IMetaWeblogApi
	{
		[XmlRpcMethod("metaWeblog.newPost")]
		string NewPost(string blogid, string username, string password, Post post, bool publish);

		[XmlRpcMethod("metaWeblog.editPost")]
		bool EditPost(string postid, string username, string password, Post post, bool publish);

		[XmlRpcMethod("metaWeblog.getPost")]
		Post GetPost(string postid, string username, string password);

		[XmlRpcMethod("metaWeblog.getCategories")]
		CategoryInfo[] GetCategories(string blogid, string username, string password);

		[XmlRpcMethod("metaWeblog.getRecentPosts")]
		Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts);

		[XmlRpcMethod("metaWeblog.newMediaObject")]
		MediaObjectInfo NewMediaObject(string blogid, string username, string password,
			MediaObject mediaObject);
	}
}