using Coolite.Ext.Web;
using Isis.Web.UI;
using Zeus.Security;

namespace Zeus.Admin.ActionPlugins
{
	public class PasteActionPlugin : ActionPluginBase
	{
		// [ActionPlugin("Paste", "Paste", Operations.Create, "CutCopyPaste", 3, null, "Zeus.Admin.Paste.aspx", "selected={selected}&memory={memory}&action={action}", Targets.Preview, "Zeus.Admin.Resources.page_paste.png", JavascriptEnableCondition = "window.top.zeus.getMemory()")]
		public override string GroupName
		{
			get { return "CutCopyPaste"; }
		}

		public override int SortOrder
		{
			get { return 3; }
		}

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Paste",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PagePaste)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Paste', '{0}&memory={{memory}}&action={{action}}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Paste.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}