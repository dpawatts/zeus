using System;
using Coolite.Ext.Web;
using Zeus.Admin.Plugins;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.Configuration;
using System.Configuration;
using Zeus.Security;
using Zeus.Web.UI;

namespace Zeus.Admin
{
	[AvailableOperation(Operations.Read, "Read", 10)]
	public partial class Default : AdminPage, IMainInterface
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

			// Allow plugins to modify interface.
			foreach (IMainInterfacePlugin plugin in Engine.ResolveAll<IMainInterfacePlugin>())
				plugin.ModifyInterface(this);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJQuery();

			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.ext.ux.menu.storemenu.js", ResourceInsertPosition.BodyBottom);

			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.zeus.js", ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.reset.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.default.css", ResourceInsertPosition.HeaderBottom);

			// Render plugin scripts.
			foreach (IMainInterfacePlugin plugin in Engine.ResolveAll<IMainInterfacePlugin>())
				plugin.RegisterScripts(ScriptManager);

			base.OnPreRender(e);
		}

		public StatusBar StatusBar
		{
			get { return stbStatusBar; }
		}

		private ScriptManager ScriptManager
		{
			get { return ScriptManager.GetInstance(this); }
		}

		public ViewPort ViewPort
		{
			get { return extViewPort; }
		}

		public BorderLayout BorderLayout
		{
			get { return extBorderLayout; }
		}

		public void LoadUserControls(string[] virtualPaths)
		{
			foreach (string virtualPath in virtualPaths)
				Controls.Add(LoadControl(virtualPath));
		}
	}
}
