using System;
using System.Collections.Generic;
using System.Linq;
using Coolite.Ext.Web;
using Isis.ExtensionMethods.Web.UI;
using Zeus.Admin.Navigation;
using Zeus.Configuration;
using System.Configuration;
using Zeus.Engine;
using Zeus.Linq;
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

			foreach (ToolbarPluginAttribute toolbarPlugin in Zeus.Context.Current.Resolve<IPluginFinder<ToolbarPluginAttribute>>().GetPlugins())
				toolbarPlugin.AddTo(plcToolbar);

			if (!Ext.IsAjaxRequest)
			{
				TreeNodeBase treeNode = SiteTree.Between(Find.StartPage, Find.RootItem, true)
					.OpenTo(Find.StartPage)
					.Filter(items => items.Authorized(Page.User, Zeus.Context.SecurityManager, Operations.Read))
					.ToTreeNode(true);
				stpNavigation.Root.Add(treeNode);
			}

			// Render action plugin user controls.
			List<string> userControlsToLoad = new List<string>();
			foreach (ActionPluginGroupAttribute actionPluginGroup in Engine.AdminManager.GetActionPluginGroups())
			{
				foreach (IActionPlugin actionPlugin in Engine.AdminManager.GetActionPlugins(actionPluginGroup.Name))
				{
					string[] requiredUserControls = actionPlugin.RequiredUserControls;
					if (requiredUserControls != null)
						userControlsToLoad.AddRange(requiredUserControls);
				}
			}

			// Render grid toolbar plugin user controls - this should actually be done by some form of plugin.
			foreach (IGridToolbarPlugin toolbarPlugin in Engine.AdminManager.GetGridToolbarPlugins())
			{
				string[] requiredUserControls = toolbarPlugin.RequiredUserControls;
				if (requiredUserControls != null)
					userControlsToLoad.AddRange(requiredUserControls);
			}

			foreach (string requiredUserControl in userControlsToLoad.Distinct())
			{
				ActionUserControlBase userControl = (ActionUserControlBase)LoadControl(requiredUserControl);
				userControl.MainInterface = this;
				Controls.Add(userControl);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJQuery();

			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.Plugins.ext.ux.menu.storemenu.js", ResourceInsertPosition.BodyBottom);

			Page.ClientScript.RegisterJavascriptResource(typeof(Default), "Zeus.Admin.Assets.JS.zeus.js", ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.reset.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.shared.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.default.css");

			// Render action plugin scripts.
			foreach (ActionPluginGroupAttribute actionPluginGroup in Engine.AdminManager.GetActionPluginGroups())
			{
				foreach (IActionPlugin actionPlugin in Engine.AdminManager.GetActionPlugins(actionPluginGroup.Name))
				{
					string[] requiredScripts = actionPlugin.RequiredScripts;
					if (requiredScripts != null)
						foreach (string requiredScript in requiredScripts)
							ScriptManager.GetInstance(this).RegisterClientScriptInclude(actionPlugin.GetType().FullName, requiredScript);
				}
			}

			base.OnPreRender(e);
		}

		#region Move

		protected void OnMoveNode(object sender, AjaxEventArgs e)
		{
			ContentItem sourceContentItem = Engine.Persister.Get(Convert.ToInt32(e.ExtraParams["source"]));
			ContentItem destinationContentItem = Engine.Persister.Get(Convert.ToInt32(e.ExtraParams["destination"]));

			// Check user has permission to create items under the SelectedItem
			if (!Engine.SecurityManager.IsAuthorized(destinationContentItem, User, Operations.Create))
			{
				Ext.MessageBox.Alert("Cannot move item", "You are not authorised to move an item to this location.");
				return;
			}

			// Change parent if necessary.
			if (sourceContentItem.Parent.ID != destinationContentItem.ID)
				Zeus.Context.Persister.Move(sourceContentItem, destinationContentItem);

			// Update sort order based on new pos.
			int pos = Convert.ToInt32(e.ExtraParams["pos"]);
			IList<ContentItem> siblings = sourceContentItem.Parent.Children;
			Utility.MoveToIndex(siblings, sourceContentItem, pos);
			foreach (ContentItem updatedItem in Utility.UpdateSortOrder(siblings))
				Zeus.Context.Persister.Save(updatedItem);

			stbStatusBar.SetStatus(new StatusBarStatusConfig("Moved item", Icon.Cut) { Clear = new StatusBarClearConfig(true) });
		}

		#endregion

		public StatusBar StatusBar
		{
			get { return stbStatusBar; }
		}
	}
}
