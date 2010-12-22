using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Xml.Serialization;
using Ext.Net;
using Newtonsoft.Json;

[assembly: WebResource("Coolite.Ext.UX.Extensions.IconCombo.resources.ext.ux.IconCombo.css", "text/css")]
[assembly: WebResource("Coolite.Ext.UX.Extensions.IconCombo.resources.ext.ux.IconCombo.js", "text/javascript")]

namespace Coolite.Ext.UX
{
	[Designer(typeof(EmptyDesigner))]
	[DefaultProperty("")]
	//[ToolboxBitmap(typeof(StoreMenu), "Extensions.GMapPanel.GMapPanel.bmp")]
	[ToolboxData("<{0}:IconCombo runat=\"server\"></{0}:IconCombo>")]
	[Description("Icon Combo")]
	public class IconCombo : ComboBox
	{
		protected override List<ResourceItem> Resources
		{
			get
			{
				List<ResourceItem> baseList = base.Resources;
				baseList.Capacity += 1;

				baseList.Add(new ClientStyleItem(typeof(IconCombo), "Coolite.Ext.UX.Extensions.IconCombo.resources.ext.ux.IconCombo.css", "ux/extensions/iconcombo/iconcombo.css"));
				baseList.Add(new ClientScriptItem(typeof(IconCombo), "Coolite.Ext.UX.Extensions.IconCombo.resources.ext.ux.IconCombo.js", "ux/extensions/iconcombo/iconcombo.js"));

				return baseList;
			}
		}

		public override string InstanceOf
		{
			get { return "Ext.ux.IconCombo"; }
		}

		public override string XType
		{
			get { return "iconcombo"; }
		}

		[DefaultValue("")]
		[Description("The underlying data field name to bind to this ComboBox (defaults to undefined if mode = \'remote\' or \'text\' if transforming a select).")]
		[Category("Config Options")]
		[ConfigOption]
		public virtual string IconUrlField
		{
			get
			{
				return (string) this.ViewState["IconUrlField"] ?? "iconUrl";
			}
			set
			{
				this.ViewState["IconUrlField"] = value;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[XmlIgnore]
		[JsonIgnore]
		public override ConfigOptionsCollection ConfigOptions
		{
			get
			{
				ConfigOptionsCollection list = base.ConfigOptions;
				list.Add("iconUrlField", string.Empty, IconUrlField);
				return list;
			}
		}

		protected override string ItemsToStore
		{
			get
			{
				StringWriter sw = new StringWriter();
				JsonTextWriter jw = new JsonTextWriter(sw);
				IconComboListItemCollectionJsonConverter converter = new IconComboListItemCollectionJsonConverter(IconUrlField);
				converter.WriteJson(jw, this.Items, null);

				return sw.GetStringBuilder().ToString();
			}
		}
	}
}