using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.Blogs.Web;
using Zeus.BaseLibrary.Web;

namespace Zeus.AddIns.Blogs.Services.Tracking
{
	/// <summary>
	/// Class used to send remote notifications, such as to Weblogs.com or a 
	/// trackback / pingback.
	/// </summary>
	internal class Notifier
	{
		public IPingbackService PingbackService { get; set; }

		public string Description { get; set; }
		public string BlogName { get; set; }
		public string Title { get; set; }
		public string FullyQualifiedUrl { get; set; }
		public Url PostUrl { get; set; }
		public string Text { get; set; }

		public void Notify(object state)
		{
			// TODO: Ping update services such as weblogs.com or technorati.com

			// Get the links from the post.
			IEnumerable<string> links = HtmlUtility.GetLinks(Text);

			// Do we have any links?
			if (!links.Any())
				return; // nothing to do

			// For each link, try to pingback, then (TODO) fallback to trackback.
			foreach (string link in links)
			{
				// Check link is valid.
				Url url;
				if (!Url.TryParse(link, out url))
					continue;

				Url pingbackUrl = PingbackService.GetPingbackUrl(url);
				if (pingbackUrl == null)
					continue;

				PingbackService.SendPing(pingbackUrl, PostUrl, url);
			}
		}
	}
}