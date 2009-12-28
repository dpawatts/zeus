using System;
using System.Collections.Generic;
using Zeus.Templates.Services.Syndication;

namespace Zeus.Templates.Mvc
{
	public class Feed
	{
		public IEnumerable<ISyndicatable> Items { get; set; }

		public bool UseDeltaEncoding { get; set; }

		/// <summary>
		/// Gets or sets the date this feed was last modified.
		/// </summary>
		/// <value></value>
		public DateTime LastModified { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether client has all feed items. 
		/// This is according to RFC3229 with feeds 
		/// <see href="http://wyman.us/main/2004/09/using_rfc3229_w.html"/>.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the client has all feed items; otherwise, <c>false</c>.
		/// </value>
		public bool ClientHasAllFeedItems { get; set; }

		public string Title { get; set; }
		public string Url { get; set; }
		public string Tagline { get; set; }
		public string Author { get; set; }
		public DateTime Published { get; set; }

		public Feed(IEnumerable<ISyndicatable> items, bool useDeltaEncoding, DateTime lastModified,
			bool clientHasAllFeedItems, string title,
			string url, string tagline, string author, DateTime published)
		{
			Items = items;
			UseDeltaEncoding = useDeltaEncoding;
			LastModified = lastModified;
			ClientHasAllFeedItems = clientHasAllFeedItems;
			Title = title;
			Url = url;
			Tagline = tagline;
			Author = author;
			Published = published;
		}
	}
}