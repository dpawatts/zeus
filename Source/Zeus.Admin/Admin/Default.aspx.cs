using System;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web.Hosting;
using Zeus.Configuration;
using System.Configuration;
using Zeus.Engine;
using Zeus.Security;
using Zeus.Web.UI;

[assembly: EmbeddedResourceFile("Zeus.Admin.Default.aspx", "Zeus.Admin")]
namespace Zeus.Admin
{
	[AvailableOperation(Operations.Read, "Read", 10)]
	public partial class Default : AdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			// These fields are used client side to store selected items
			Page.ClientScript.RegisterHiddenField("selected", SelectedItem.Path);
			Page.ClientScript.RegisterHiddenField("memory", string.Empty);
			Page.ClientScript.RegisterHiddenField("action", string.Empty);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			imgLogo.Src = ClientScript.GetWebResourceUrl(typeof(Default), "Zeus.Admin.Assets.Images.Theme.logo.gif");
			imgLogo.Visible = !((AdminSection) ConfigurationManager.GetSection("zeus/admin")).HideBranding;
			ltlAdminName1.Text = ltlAdminName2.Text = ((AdminSection) ConfigurationManager.GetSection("zeus/admin")).Name;

			foreach (ToolbarPluginAttribute toolbarPlugin in Zeus.Context.Current.Resolve<IPluginFinder<ToolbarPluginAttribute>>().GetPlugins())
				toolbarPlugin.AddTo(plcToolbar);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJQuery();
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.jquery.splitter.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.zeus.js", ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.reset.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.default.css");
			base.OnPreRender(e);
		}
	}
}
