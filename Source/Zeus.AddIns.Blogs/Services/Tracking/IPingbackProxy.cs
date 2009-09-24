using CookComputing.XmlRpc;

namespace Zeus.AddIns.Blogs.Services.Tracking
{
	public interface IPingbackProxy : IXmlRpcProxy
	{
		[XmlRpcMethod("pingback.ping")]
		string Ping(string source, string destination);
	}
}