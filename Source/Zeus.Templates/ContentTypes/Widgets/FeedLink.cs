using Coolite.Ext.Web;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes.Widgets
{
	[ContentType("Feed Link", Description = "An RSS feed subscription link. An RSS link is also added to the page header, enabling subscription through the browser's built-in RSS handling.")]
	[RestrictParents(typeof(PageContentItem))]
	[AllowedZones(AllowedZones.AllNamed)]
	public class FeedLink : WidgetContentItem
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.FeedLink); }
		}

		[ContentProperty("Feed", 200)]
		public virtual RssFeed SelectedFeed
		{
			get { return GetDetail<RssFeed>("SelectedFeed", null); }
			set { SetDetail("SelectedFeed", value); }
		}
	}
}