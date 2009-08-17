using Isis.Web.UI;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Globalization;
using Zeus.Web.UI;

namespace Zeus.Templates.ContentTypes
{
	[TabPanel("Content", "Content", 10)]
	[Translatable]
	[DefaultContainer("Content")]
	public abstract class BaseContentItem : ContentItem
	{
		[TextBoxEditor("Title", 10, Required = true, Shared = false)]
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