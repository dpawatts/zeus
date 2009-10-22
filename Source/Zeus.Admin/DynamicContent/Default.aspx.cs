using System;
using System.Linq;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.Configuration;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.DynamicContent;
using Zeus.Web.Hosting;
using Zeus.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.Admin.DynamicContent
{
	public partial class Default : System.Web.UI.Page
	{
		private IDynamicContentManager Manager
		{
			get { return Zeus.Context.Current.Resolve<IDynamicContentManager>(); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ddlPlugin.DataSource = Manager.GetAvailableControls();
				ddlPlugin.DataBind();
			}

			ChangeSelectedPlugin(ddlPlugin.SelectedValue);
		}

		private void ChangeSelectedPlugin(string name)
		{
			DynamicContentControl control = Manager.GetAvailableControls().Single(dcc => dcc.Name == ddlPlugin.SelectedValue);
			lblDescription.Text = control.Description ?? control.Name;

			IDynamicContent dynamicContent = Manager.InstantiateDynamicContent(control.Name);
			zeusItemEditor.CurrentItemDefinition = new SimpleTypeDefinition(
				new EditableHierarchyBuilder<IEditor>(),
				new AttributeExplorer<IEditorContainer>(),
				new AttributeExplorer<IEditor>(),
				dynamicContent);
			zeusItemEditor.CurrentItem = new SimpleEditableObject(dynamicContent);
		}

		protected void ddlPlugin_SelectedIndexChanged(object sender, EventArgs e)
		{
			ChangeSelectedPlugin(ddlPlugin.SelectedValue);
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			// Populate properties on dynamic content.
			IDynamicContent dynamicContent = (IDynamicContent) ((SimpleEditableObject) zeusItemEditor.Save()).WrappedObject;

			// Return HTML string to TinyMCE editor.
			ltlRenderedContentElement.Text = Manager.GetMarkup(dynamicContent);

			plcStep1.Visible = false;
			plcStep2.Visible = true;
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJQuery();
			Page.ClientScript.RegisterJavascriptInclude(Utility.GetClientResourceUrl(typeof(ContentItem), "TinyMCE/tiny_mce_popup.js"), ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.reset.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.preview.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.edit.css");
			base.OnPreRender(e);
		}
	}
}
