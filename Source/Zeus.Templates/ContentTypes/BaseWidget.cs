using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Web;
using Zeus.Web.UI;

namespace Zeus.Templates.ContentTypes
{
	[TabPanel("General", "General", 100)]
	public abstract class BaseWidget : WidgetContentItem
	{
		[TextBoxEditor("Title", 10, ContainerName = "General")]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override Icon Icon
		{
			get { return Icon.PageWhite; }
		}
	}
}