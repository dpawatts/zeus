using Zeus.AddIns.Forums.ContentTypes;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.FileSystem.Images;
using Zeus.Templates.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	[ContentType("Page")]
	//[ReplacesParentContentType]
	public class CustomPage : Page, IMessageBoardContainer
	{
		[XhtmlStringContentProperty("Content", 30, Shared = false)]
		public override string Content
		{
			get { return base.Content; }
			set { base.Content = value; }
		}

		/*[ContentProperty("Image", 100, Shared = false)]
		[ChildEditor]
		public Image Image
		{
			get { return GetDetail<Image>("Image", null); }
			set { SetDetail("Image", value); }
		}*/

		/*[MultiImageDataUploadEditor("Images", 110)]
		public PropertyCollection Images
		{
			get { return GetDetailCollection("Images", true); }
		}*/
	}
}