using CookComputing.XmlRpc;

namespace Zeus.AddIns.Blogs.Web.Pingbacks
{
	public class PingbackSender
	{
		#region Public Methods

		public WeblogUpdatesPingResponse Ping(string title, string url)
		{
			IWeblogUpdates rpcProxy = (IWeblogUpdates)XmlRpcProxyGen.Create(typeof(IWeblogUpdates));
			return rpcProxy.Ping(title, url);
		}

		#endregion
	}
}