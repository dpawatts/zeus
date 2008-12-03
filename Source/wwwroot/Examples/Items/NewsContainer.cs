using System;
using Zeus.Integrity;

namespace Zeus.Templates.Items
{
	[Definition("News Container", "NewsContainer", "A list of news. News items can be added to this page.", "", 150)]
	public class NewsContainer : ContentItem
	{
		public override string IconUrl
		{
			get { return "newspaper_link"; }
		}

		public override string TemplateUrl
		{
			get { return "NewsList"; }
		}
	}
}
