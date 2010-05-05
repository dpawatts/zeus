using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Web;
using Zeus.Web.UI;

namespace Zeus.Templates.ContentTypes
{
	public abstract class BaseWidget : WidgetContentItem
	{
		[TextBoxEditor("Title", 10)]
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