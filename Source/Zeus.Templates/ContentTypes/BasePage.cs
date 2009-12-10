using Coolite.Ext.Web;
using Zeus.Templates.Design.Editors;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	[DefaultTemplate]
	public abstract class BasePage : PageContentItem
	{
		[ContentProperty("Page Title", 11, Description = "Used in the &lt;h1&gt; element on the page")]
		[PageTitleEditor]
		public virtual string PageTitle
		{
			get { return GetDetail("PageTitle", Title); }
			set { SetDetail("PageTitle", value); }
		}
	}
}