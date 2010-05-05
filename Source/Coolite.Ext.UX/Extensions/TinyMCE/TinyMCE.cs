using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Xml.Serialization;
using Ext.Net;
using Newtonsoft.Json;

//[assembly: WebResource("Coolite.Ext.UX.Extensions.IconCombo.resources.ext.ux.IconCombo.css", "text/css")]
[assembly: WebResource("Coolite.Ext.UX.Extensions.TinyMCE.resources.Ext.ux.TinyMCE.js", "text/javascript")]

namespace Coolite.Ext.UX
{
	[Designer(typeof(EmptyDesigner))]
	[DefaultProperty("")]
	//[ToolboxBitmap(typeof(StoreMenu), "Extensions.GMapPanel.GMapPanel.bmp")]
	[ToolboxData("<{0}:TinyMCE runat=\"server\"></{0}:TinyMCE>")]
	[Description("TinyMCE Text Area")]
	public class TinyMCE : Field
	{
		#region Properties

		protected override List<ResourceItem> Resources
		{
			get
			{
				List<ResourceItem> baseList = base.Resources;
				baseList.Capacity += 1;

				baseList.Add(new ClientScriptItem(typeof(TinyMCE), "Coolite.Ext.UX.Extensions.TinyMCE.resources.Ext.ux.TinyMCE.js", "ux/extensions/tinymce/tinymce.js"));

				return baseList;
			}
		}

		public override string InstanceOf
		{
			get { return "Ext.ux.TinyMCE"; }
		}

		public override string XType
		{
			get { return "tinymce"; }
		}

		/// <summary>
		/// The Text value to initialize this field with.
		/// </summary>
		[Meta]
		[DefaultValue("")]
		[Category("Appearance")]
		[Localizable(true)]
		[Themeable(false)]
		[Bindable(true, BindingDirection.TwoWay)]
		[Description("The Text value to initialize this field with.")]
		public virtual string Text
		{
			get { return (string) this.Value ?? ""; }
			set { this.Value = value == "" ? null : value; }
		}

		/// <summary>
		/// The default text to display in an empty field (defaults to null).
		/// </summary>
		[Meta]
		[ConfigOption]
		[Category("6. TextField")]
		[DefaultValue("")]
		[Localizable(true)]
		[Description("The default text to display in an empty field (defaults to null).")]
		public virtual string EmptyText
		{
			get { return (string) this.ViewState["EmptyText"] ?? ""; }
			set { this.ViewState["EmptyText"] = value; }
		}

		private TinyMCESettings _settings;

		/// <summary>
		/// Client-side JavaScript Event Handlers
		/// </summary>
		[Meta]
		[ConfigOption("tinymceSettings", JsonMode.Object)]
		[Category("2. Observable")]
		[Themeable(false)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Description("Client-side JavaScript Event Handlers")]
		[ViewStateMember]
		public TinyMCESettings Settings
		{
			get
			{
				if (_settings == null)
					_settings = new TinyMCESettings();

				return _settings;
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
				list.Add("tinymceSettings", new ConfigOption("tinymceSettings", new SerializationOptions("tinymceSettings", JsonMode.Object), null, Settings));
				return list;
			}
		}

		#endregion

		#region Methods

		private static readonly object EventTextChanged = new object();

		/// <summary>
		/// Fires when the Text property has been changed.
		/// </summary>
		[Category("Action")]
		[Description("Fires when the Text property has been changed.")]
		public event EventHandler TextChanged
		{
			add { Events.AddHandler(EventTextChanged, value); }
			remove { Events.RemoveHandler(EventTextChanged, value); }
		}

		protected virtual void OnTextChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler) Events[EventTextChanged];
			if (handler != null)
				handler(this, e);
		}

		protected override void RaisePostDataChangedEvent()
		{
			OnTextChanged(EventArgs.Empty);
		}

		protected override bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string val = postCollection[UniqueName];

			if (val != null && Text != val)
			{
				bool raise = val != (Text ?? string.Empty);
				try
				{
					ViewState.Suspend();
					Text = val.Equals(EmptyText) ? string.Empty : val;
				}
				finally
				{
					ViewState.Resume();
				}

				return raise;
			}

			return false;
		}

		#endregion
	}
}