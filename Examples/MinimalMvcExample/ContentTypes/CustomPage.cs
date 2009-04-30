using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Templates.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	[ContentType("Page")]
	[ReplacesParentContentType]
	public class CustomPage : Page
	{
		[XhtmlStringContentProperty("Content", 30, Shared = false)]
		public override string Content
		{
			get { return base.Content; }
			set { base.Content = value; }
		}
	}
}