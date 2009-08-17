using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;

namespace Zeus.AddIns.Blogs.Web.Trackbacks
{
	public class TrackbackSender
	{
		#region Public Constructor

		public TrackbackSender(string blogTitle, Uri destinationUrl)
		{
			BlogTitle = blogTitle;
			DestinationUrl = destinationUrl;
		}

		#endregion

		#region Properties

		public string BlogTitle { get; set; }
		public Uri DestinationUrl { get; set; }

		#endregion

		#region Public Methods

		public void SendTrackback(Uri url, string title, string excerpt)
		{
			NameValueCollection data = new NameValueCollection();
			data.Add("blog_name", BlogTitle);
			data.Add("url", url.ToString());
			data.Add("title", title);
			data.Add("excerpt", excerpt);

			WebClient webClient = new WebClient();
			webClient.UploadValues(url, data);
		}

		#endregion
	}
}