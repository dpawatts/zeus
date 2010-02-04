using Ext.Net;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.News
{
	[ContentType("Latest News Summary")]
	[AllowedZones(AllowedZones.AllNamed)]
	public class LatestNewsSummary : BaseWidget
	{
		protected override Icon Icon
		{
			get { return Icon.Newspaper; }
		}

		[ContentProperty("News Section", 200)]
		public NewsContainer NewsSection
		{
			get { return GetDetail<NewsContainer>("NewsSection", null); }
			set { SetDetail("NewsSection", value); }
		}

		[ContentProperty("# To Show", 210)]
		public int NumberToShow
		{
			get { return GetDetail("NumberToShow", 3); }
			set { SetDetail("NumberToShow", value); }
		}
	}
}