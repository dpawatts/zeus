using System.ComponentModel;
using System.IO;
using System.Web.UI;
using Coolite.Ext.Web;
using Newtonsoft.Json;

[assembly: WebResource("Coolite.Ext.UX.Extensions.IconCombo.resources.ext.ux.IconCombo.css", "text/css")]
[assembly: WebResource("Coolite.Ext.UX.Extensions.IconCombo.resources.ext.ux.IconCombo.js", "text/javascript")]

namespace Coolite.Ext.UX
{
	[Designer(typeof(EmptyDesigner))]
	[DefaultProperty("")]
	[Xtype("iconcombo")]
	[InstanceOf(ClassName = "Ext.ux.IconCombo")]
	[ClientStyle(Type = typeof(IconCombo), WebResource = "Coolite.Ext.UX.Extensions.IconCombo.resources.ext.ux.IconCombo.css", FilePath = "/ux/extensions/iconcombo/iconcombo.css")]
	[ClientScript(Type = typeof(IconCombo), WebResource = "Coolite.Ext.UX.Extensions.IconCombo.resources.ext.ux.IconCombo.js", FilePath = "ux/extensions/iconcombo/iconcombo.js")]
	//[ToolboxBitmap(typeof(StoreMenu), "Extensions.GMapPanel.GMapPanel.bmp")]
	[ToolboxData("<{0}:IconCombo runat=\"server\"></{0}:IconCombo>")]
	[Description("Icon Combo")]
	public class IconCombo : ComboBox
	{
		[DefaultValue("")]
		[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		[Category("Config Options")]
		[ClientConfig]
		public virtual string IconClsField
		{
			get
			{
				return (string) this.ViewState["IconClsField"] ?? "iconCls";
			}
			set
			{
				this.ViewState["IconClsField"] = value;
			}
		}

		protected override string ItemsToStore
		{
			get
			{
				StringWriter sw = new StringWriter();
				JsonTextWriter jw = new JsonTextWriter(sw);
				IconComboListItemCollectionJsonConverter converter = new IconComboListItemCollectionJsonConverter(IconClsField);
				converter.WriteJson(jw, this.Items);

				return sw.GetStringBuilder().ToString();
			}
		}
	}
}