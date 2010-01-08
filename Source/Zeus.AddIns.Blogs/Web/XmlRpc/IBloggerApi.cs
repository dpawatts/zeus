using CookComputing.XmlRpc;

namespace Zeus.AddIns.Blogs.Web.XmlRpc
{
	public interface IBloggerApi
	{
		[XmlRpcMethod("blogger.deletePost")]
		[return: XmlRpcReturnValue(Description = "Returns true.")]
		bool DeletePost(string key, string postid, string username, string password, bool publish);

		[XmlRpcMethod("blogger.getUsersBlogs")]
		BlogInfo[] GetUsersBlogs(string key, string username, string password);

		[XmlRpcMethod("blogger.getUserInfo")]
		UserInfo GetUserInfo(string key, string username, string password);
	}
}