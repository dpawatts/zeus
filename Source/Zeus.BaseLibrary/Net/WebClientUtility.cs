using System.Net;

namespace Isis.Net
{
	public class WebClientUtility
	{
		public static string GetHtml(string url)
		{
			using (WebClient webClient = new WebClient())
			{
				return webClient.DownloadString(url);
			}
		}
	}
}