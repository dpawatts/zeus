using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	public class TabItem : Panel, INamingContainer
	{
		public TabItem()
		{
			CssClass = "tabPanel";
		}
	}
}
