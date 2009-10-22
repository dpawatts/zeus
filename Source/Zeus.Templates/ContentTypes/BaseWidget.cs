using Zeus.BaseLibrary.Web.UI;
using Zeus.Design.Editors;
using Zeus.Web.UI;

namespace Zeus.Templates.ContentTypes
{
	[TabPanel("General", "General", 100)]
	public abstract class BaseWidget : ContentItem
	{
		[TextBoxEditor("Title", 10, ContainerName = "General")]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(BaseContentItem), "Zeus.Templates.Icons." + IconName + ".png"); }
		}

		protected virtual string IconName
		{
			get { return "page_white"; }
		}

		public override bool IsPage
		{
			get { return false; }
		}
	}
}