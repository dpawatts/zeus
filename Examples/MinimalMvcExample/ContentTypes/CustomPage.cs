using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.FileSystem.Images;
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

		[ContentProperty("Image", 100, Shared = false)]
		public ImageData Image
		{
			get { return GetDetail<ImageData>("Image", null); }
			set { SetDetail("Image", value); }
		}
	}
}