using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.Widgets
{
	[ContentType("Feed Link", Description = "An RSS feed subscription link. An RSS link is also added to the page header, enabling subscription through the browser's built-in RSS handling.")]
	[RestrictParents(typeof(BasePage))]
	[AllowedZones(AllowedZones.AllNamed)]
	public class FeedLink : BaseWidget
	{
		protected override string IconName
		{
			get { return "feed_link"; }
		}

		[ContentProperty("Feed", 200)]
		public virtual RssFeed SelectedFeed
		{
			get { return GetDetail<RssFeed>("SelectedFeed", null); }
			set { SetDetail("SelectedFeed", value); }
		}
	}
}