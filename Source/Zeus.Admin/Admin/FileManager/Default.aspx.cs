using System;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web.Hosting;
using Zeus.Web.UI;

[assembly: EmbeddedResourceFile("Zeus.Admin.FileManager.Default.aspx", "Zeus.Admin")]
namespace Zeus.Admin.FileManager
{
	public partial class Default : System.Web.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Request.QueryString["destinationType"] == "image")
					ftrFileTree.RootPath = Request.QueryString["rootPath"];
			}
			base.OnLoad(e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJQuery();
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.jquery.simpleTree.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.jquery.easing.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.jquery.easing.compatibility.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.jquery.dimensions.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.jquery.contextMenu.js");
			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.thickbox.js");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterEmbeddedCssResource(typeof(Default), "Zeus.Admin.Assets.Css.tree.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.thickbox.css");
			base.OnPreRender(e);
		}
	}
}
