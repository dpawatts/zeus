using Zeus.ContentProperties;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.Widgets
{
	[ContentType("Text Item")]
	[AllowedZones(AllowedZones.AllNamed)]
	public class TextItem : BaseContentItem
	{
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[XhtmlStringContentProperty("Text", 100)]
		public virtual string Text
		{
			get { return GetDetail("Text", string.Empty); }
			set { SetDetail("Text", value); }
		}
	}
}