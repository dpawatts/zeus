using System;
using CookComputing.XmlRpc;

namespace Zeus.AddIns.Blogs.Web.Pingbacks
{
	public class PingbackHandler : XmlRpcService
	{
		[XmlRpcMethod("pingback.ping")]
		public string pingback(string sourceUri, string targetUri)
		{
			try
			{
				throw new NotImplementedException();

				// Find the item ID or item name
				// Check for the existence of a page at source URI
				// Check for the existence of a link to the target URI in the source URI
				// Store the pingback on local server for the appropriate item
			}
			catch
			{
				throw new XmlRpcFaultException(1, "Invalid sourceUri parameter.");
			}

			return "Your ping request has been received successfully.";
		}
	}
}