using System.ComponentModel;
using Ext.Net;

namespace Coolite.Ext.UX
{
	public class IconComboListItem : ListItem
	{
		[DefaultValue("")]
		[NotifyParentProperty(true)]
		public string IconUrl
		{
			get { return (string) this.ViewState["IconUrl"] ?? ""; }
			set { this.ViewState["IconUrl"] = value; }
		}

		public IconComboListItem(string text, string value, string iconUrl)
			: base(text, value)
		{
			IconUrl = iconUrl;
		}
	}
}