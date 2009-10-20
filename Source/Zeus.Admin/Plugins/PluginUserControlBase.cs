using Coolite.Ext.Web;
using Zeus.Engine;

namespace Zeus.Admin.Plugins
{
	public abstract class PluginUserControlBase : System.Web.UI.UserControl
	{
		protected ContentEngine Engine
		{
			get { return Zeus.Context.Current; }
		}

		public IMainInterface MainInterface
		{
			get;
			set;
		}

		protected void Refresh(ContentItem item)
		{
			ScriptManager.GetInstance(Page).AddScript("zeus.refresh('{0}', '{1}');", item.ID, item.Url);
		}
	}
}