using System.Net;

namespace Zeus.BaseLibrary.Net
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