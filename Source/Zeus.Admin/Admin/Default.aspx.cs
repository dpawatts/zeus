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
			//Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.jquery.splitter.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.zeus.js", ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.reset.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.default.css");

			// Mocha UI Theme
			const string MOCHA_UI_THEME = "default";
			Page.ClientScript.RegisterEmbeddedCssResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Themes." + MOCHA_UI_THEME + ".css.Content.css");
			Page.ClientScript.RegisterEmbeddedCssResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Themes." + MOCHA_UI_THEME + ".css.Core.css");
			Page.ClientScript.RegisterEmbeddedCssResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Themes." + MOCHA_UI_THEME + ".css.Layout.css");
			Page.ClientScript.RegisterEmbeddedCssResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Themes." + MOCHA_UI_THEME + ".css.Dock.css");
			Page.ClientScript.RegisterEmbeddedCssResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Themes." + MOCHA_UI_THEME + ".css.Window.css");
			Page.ClientScript.RegisterEmbeddedCssResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Themes." + MOCHA_UI_THEME + ".css.Tabs.css");

			// Mocha UI Scripts
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Scripts.excanvas_r43.js", "<!--[if IE]>" + Environment.NewLine, Environment.NewLine + "<![endif]-->");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Scripts.mootools-1.2-core.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Scripts.mootools-1.2-more.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Scripts.Source.Core.Core.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Scripts.Source.Layout.Layout.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Scripts.Source.Layout.Dock.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Scripts.Source.Window.Window.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Scripts.Source.Window.Modal.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.MochaUI.Scripts.Source.Components.Tabs.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.mocha-init.js");

			base.OnPreRender(e);
		}
	}
}
