using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(StartPage), typeof(Page), typeof(Redirect))]
	public class Page : BasePage
	{
		[XhtmlStringContentProperty("Content", 30)]
		public virtual string Content
		{
			get { return GetDetail("Content", string.Empty); }
			set { SetDetail("Content", value); }
		}
	}
}