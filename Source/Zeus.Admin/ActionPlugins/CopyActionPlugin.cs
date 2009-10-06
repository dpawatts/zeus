using Coolite.Ext.Web;
using Isis.Web.UI;
using Zeus.Security;

namespace Zeus.Admin.ActionPlugins
{
	public class CopyActionPlugin : ActionPluginBase
	{
		public override string GroupName
		{
			get { return "CutCopyPaste"; }
		}

		public override int SortOrder
		{
			get { return 2; }
		}

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Copy",
				IconUrl = Utility.GetCooliteIconUrl(Icon.PageCopy)
			};

			menuItem.Handler = string.Format("function() {{ zeus.memorize('{0}', 'Copy.aspx'); }}",
				contentItem.Path);

			return menuItem;
		}
	}
}