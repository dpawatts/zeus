using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.FileSystem;
using Zeus.FileSystem.Images;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(WebsiteNode), typeof(Page), typeof(Redirect))]
	[AllowedChildren(typeof(File), typeof(Image))]
	public class Page : BasePage
	{
		[XhtmlStringContentProperty("Content", 30, Shared = false), HtmlTextBoxEditor(ContainerName = "Content")]
		public virtual string Content
		{
			get { return GetDetail("Content", string.Empty); }
			set { SetDetail("Content", value); }
		}
	}
}