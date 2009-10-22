using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using CookComputing.XmlRpc;
using Zeus.BaseLibrary.Web;

namespace Zeus.AddIns.Blogs.Services.Tracking
{
	/// <summary>
	/// Implements http://hixie.ch/specs/pingback/pingback-1.0.
	/// </summary>
	public class PingbackService : IPingbackService
	{
		public Url GetPingbackUrl(Url destination)
		{
			HttpWebResponse webResponse = null;
			try
			{
				// Get response from destination.
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(destination);
				webResponse = (HttpWebResponse)webRequest.GetResponse();

				// First, check for the X-Pingback header.
				string result = webResponse.Headers["X-Pingback"];
				if (!string.IsNullOrEmpty(result))
					return result;

				// Otherwise, check for <link> element - note that the specification doesn't say 
				// we should first check for text/html content type.
				string entityBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
				const string pattern = "<link rel=\"pingback\" href=\"([^\"]+)\" ?/?>";
				var reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
				Match match = reg.Match(entityBody);
				if (match.Success)
					return match.Result("$1");
			}
			catch
			{
				// We don't care that much about exceptions here.
			}
			finally
			{
				// Clean up.
				if (webResponse != null)
					webResponse.Close();
			}

			return null;
		}

		public string SendPing(Url pingbackUrl, Url source, Url destination)
		{
			IPingbackProxy pingbackProxy = XmlRpcProxyGen.Create<IPingbackProxy>();
			pingbackProxy.Url = pingbackUrl;
			return pingbackProxy.Ping(source, destination);
		}
	}
}