using System.ComponentModel;
using Coolite.Ext.Web;

namespace Coolite.Ext.UX
{
	public class IconComboListItem : ListItem
	{
		[DefaultValue("")]
		[NotifyParentProperty(true)]
		public string IconCls
		{
			get { return (string) this.ViewState["IconCls"] ?? ""; }
			set { this.ViewState["IconCls"] = value; }
		}

		public IconComboListItem(string text, string value, string iconCls)
			: base(text, value)
		{
			IconCls = iconCls;
		}
	}
}