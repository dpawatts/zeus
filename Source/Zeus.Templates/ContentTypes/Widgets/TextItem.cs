using Zeus.ContentProperties;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes.Widgets
{
	[ContentType("Text Item")]
	[AllowedZones(AllowedZones.AllNamed)]
	public class TextItem : WidgetContentItem
	{
		[XhtmlStringContentProperty("Text", 100)]
		public virtual string Text
		{
			get { return GetDetail("Text", string.Empty); }
			set { SetDetail("Text", value); }
		}
	}
}