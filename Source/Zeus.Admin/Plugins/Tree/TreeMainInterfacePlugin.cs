using Coolite.Ext.Web;
using Zeus.Linq;
using Zeus.Security;
using Zeus.Web.Hosting;
using Zeus.BaseLibrary.Web.UI;

[assembly: System.Web.UI.WebResource("Zeus.Admin.Plugins.Tree.Resources.TreeCssInitializer.js", "text/javascript")]

namespace Zeus.Admin.Plugins.Tree
{
	public class TreeMainInterfacePlugin : MainInterfacePluginBase
	{
		public override void ModifyInterface(IMainInterface mainInterface)
		{
			// Setup West panel.
			mainInterface.BorderLayout.West.Split = true;
			mainInterface.BorderLayout.West.MinWidth = 175;
			mainInterface.BorderLayout.West.MaxWidth = 400;

			// Add tree.
			TreePanel treePanel = new TreePanel
			{
				ID = "stpNavigation",
				Width = 200,
				Icon = Icon.SitemapColor,
				Title = "Site",
				AutoScroll = true,
				PathSeparator = "|",
				EnableDD = true
			};
			mainInterface.BorderLayout.West.Items.Add(treePanel);

			// Setup tree top toolbar.
			Toolbar topToolbar = new Toolbar();
			treePanel.TopBar.Add(topToolbar);
			topToolbar.Items.Add(new ToolbarFill());

			ToolbarButton refreshButton = new ToolbarButton { Icon = Icon.Reload };
			refreshButton.ToolTips.Add(new ToolTip { Html = "Refresh" });
			refreshButton.Listeners.Click.Handler = string.Format("{0}.getLoader().load({0}.getRootNode());", treePanel.ClientID);
			topToolbar.Items.Add(refreshButton);

			ToolbarButton expandAllButton = new ToolbarButton { IconCls = "icon-expand-all" };
			expandAllButton.ToolTips.Add(new ToolTip { Html = "Expand All" });
			expandAllButton.Listeners.Click.Handler = string.Format("{0}.expandAll();", treePanel.ClientID);
			topToolbar.Items.Add(expandAllButton);

			ToolbarButton collapseAllButton = new ToolbarButton { IconCls = "icon-collapse-all" };
			collapseAllButton.ToolTips.Add(new ToolTip { Html = "Collapse All" });
			collapseAllButton.Listeners.Click.Handler = string.Format("{0}.collapseAll();", treePanel.ClientID);
			topToolbar.Items.Add(collapseAllButton);

			// Data loader.
			treePanel.Loader.Add(new Coolite.Ext.Web.TreeLoader
			{
				DataUrl = Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(GetType().Assembly, "Zeus.Admin.Plugins.Tree.TreeLoader.ashx"),
				PreloadChildren = true
			});

			// Call tree modification plugins and load tree plugin user controls.
			foreach (ITreePlugin treePlugin in Context.Current.ResolveAll<ITreePlugin>())
			{
				string[] requiredUserControls = treePlugin.RequiredUserControls;
				if (requiredUserControls != null)
					mainInterface.LoadUserControls(requiredUserControls);

				treePlugin.ModifyTree(treePanel, mainInterface);
			}

			if (!Ext.IsAjaxRequest)
			{
				TreeNodeBase treeNode = SiteTree.Between(Find.StartPage, Find.RootItem, true)
					.OpenTo(Find.StartPage)
					.Filter(items => items.Authorized(Context.Current.WebContext.User, Context.SecurityManager, Operations.Read))
					.ToTreeNode(true);
				treePanel.Root.Add(treeNode);
			}
		}

		public override void RegisterScripts(ScriptManager scriptManager)
		{
			scriptManager.RegisterClientScriptInclude("TreeCssInitializer",
				WebResourceUtility.GetUrl(GetType(), "Zeus.Admin.Plugins.Tree.Resources.TreeCssInitializer.js"));

			// Call tree modification plugins.
			foreach (ITreePlugin treePlugin in Context.Current.ResolveAll<ITreePlugin>())
			{
				string[] requiredScripts = treePlugin.RequiredScripts;
				if (requiredScripts != null)
					foreach (string requiredScript in requiredScripts)
						scriptManager.RegisterClientScriptInclude(treePlugin.GetType().FullName, requiredScript);
			}
		}
	}
}