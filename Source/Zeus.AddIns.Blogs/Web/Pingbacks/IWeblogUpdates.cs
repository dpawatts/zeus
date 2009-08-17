using CookComputing.XmlRpc;

namespace Zeus.AddIns.Blogs.Web.Pingbacks
{
	[XmlRpcUrl("http://nayyeri.net/sample-post")]
	public interface IWeblogUpdates
	{
		[XmlRpcMethod("weblogUpdates.ping")]
		WeblogUpdatesPingResponse Ping(string title, string url);
	}
}