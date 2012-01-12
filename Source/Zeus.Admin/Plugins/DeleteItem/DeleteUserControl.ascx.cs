using System;
using System.Linq;
using Ext.Net;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins.DeleteItem
{
	[DirectMethodProxyID(Alias = "Delete", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class DeleteUserControl : PluginUserControlBase
	{
		[DirectMethod]
		public void ShowDialog(string title, string subtitle, string affectedItemIDs)
		{
			var window = new Window
			{
				ID = "deleteDialog",
				Modal = true,
				Width = 500,
				Height = 300,
				Title = title,
				Layout = "fit",
				Maximizable = true
			};

			window.Listeners.Maximize.Fn = "function(el) { var v = Ext.getBody().getViewSize(); el.setSize(v.width, v.height); }";
			window.Listeners.Maximize.Scope = "this";

			FormPanel formPanel = new FormPanel
			{
				BaseCls = "x-plain",
				Layout = "absolute"
			};
			window.Items.Add(formPanel);

			formPanel.ContentControls.Add(new Label
			{
				Html = @"<div class=""x-window-dlg""><div class=""ext-mb-warning"" style=""width:32px;height:32px""></div></div>",
				X = 5,
				Y = 5
			});
			formPanel.ContentControls.Add(new Label
			{
				Html = subtitle,
				X = 42,
				Y = 6
			});

			TabPanel tabPanel = new TabPanel
			{
				ID = "deleteDialog_TabPanel",
				X = 0,
				Y = 42,
				Anchor = "100% 100%",
				AutoTabs = true,
				DeferredRender = false,
				Border = false
			};
			formPanel.ContentControls.Add(tabPanel);

			TreePanel affectedItemsTreePanel = new TreePanel
			{
				Title = "Affected Items",
				AutoScroll = true,
				RootVisible = false
			};
			tabPanel.Items.Add(affectedItemsTreePanel);

			TreeLoader treeLoader = new TreeLoader
			{
				DataUrl = Engine.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(typeof(DeleteUserControl).Assembly,
					"Zeus.Admin.Plugins.DeleteItem.AffectedItems.ashx")
			};
			affectedItemsTreePanel.Loader.Add(treeLoader);

			treeLoader.Listeners.Load.Fn = "function(loader, node) { node.getOwnerTree().expandAll(); }";

			affectedItemsTreePanel.Root.Add(new AsyncTreeNode
			{
				Text = "Root",
				NodeID = affectedItemIDs,
				Expanded = true
			});

			TreePanel referencingItemsTreePanel = new TreePanel
			{
				Title = "Referencing Items",
				TabTip = "Items referencing the item(s) you're deleting",
				AutoScroll = true,
				RootVisible = false
			};
			tabPanel.Items.Add(referencingItemsTreePanel);

			TreeLoader referencingItemsTreeLoader = new TreeLoader
			{
				DataUrl = Engine.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(typeof(DeleteUserControl).Assembly,
					"Zeus.Admin.Plugins.DeleteItem.ReferencingItems.ashx")
			};
			referencingItemsTreePanel.Loader.Add(referencingItemsTreeLoader);

			referencingItemsTreePanel.Root.Add(new AsyncTreeNode
			{
				Text = "Root",
				NodeID = affectedItemIDs,
				Expanded = true
			});

			Button yesButton = new Button
			{
				ID = "yesButton",
				Text = "Yes"
			};
			yesButton.Listeners.Click.Handler = string.Format(@"
				stbStatusBar.showBusy('Deleting...');
				{0}.hide();
				Ext.net.DirectMethods.Delete.DeleteItems('{1}',
				{{
					url : '{2}',
					success: function() {{ stbStatusBar.setStatus({{ text: 'Deleted Item(s)', iconCls: '', clear: true }}); }}
				}});",
				window.ClientID, affectedItemIDs, Engine.AdminManager.GetAdminDefaultUrl());
			window.Buttons.Add(yesButton);

			window.Buttons.Add(new Button
			{
				ID = "noButton",
				Text = "No",
				Handler = string.Format(@"function() {{ {0}.hide(); }}", window.ClientID)
			});

			window.Render(pnlContainer, RenderMode.RenderTo);
		}

		[DirectMethod]
		public void DeleteItems(string ids)
		{
			if (string.IsNullOrEmpty(ids))
				return;

			string[] nodeIDsTemp = ids.Split(',');
			var nodeIDs = nodeIDsTemp.Select(s => Convert.ToInt32(s));
			if (!nodeIDs.Any())
				return;

			ContentItem parent = Engine.Persister.Get(nodeIDs.First()).Parent;
			foreach (int id in nodeIDs)
			{
				ContentItem item = Engine.Persister.Get(id);
				Zeus.Context.Persister.Delete(item);
			}

            //set the updated value on the parent of the item that has been moved (for caching purposes)
            parent.Updated = Utility.CurrentTime();
            Zeus.Context.Persister.Save(parent);

            ContentItem theParent = parent;
            while (theParent.Parent != null)
            {
                //go up the tree updating - if a child has been changed, so effectively has the parent
                theParent.Updated = DateTime.Now;
                Zeus.Context.Persister.Save(theParent);
                theParent = theParent.Parent;
            }

			if (parent != null)
				Refresh(parent);
			else
				Refresh(Zeus.Context.UrlParser.StartPage);
		}
	}
}