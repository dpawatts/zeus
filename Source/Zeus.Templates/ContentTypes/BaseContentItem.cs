using Isis.Web.UI;
using Zeus.Design.Editors;
using Zeus.Web.UI;

namespace Zeus.Templates.ContentTypes
{
	[TabPanel("Content", "Content", 10)]
	public abstract class BaseContentItem : ContentItem
	{
		[TextBoxEditor("Title", 10, Required = true, ContainerName = "Content", Shared = false)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[NameEditor("Name", 20, Required = true, ContainerName = "Content")]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
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