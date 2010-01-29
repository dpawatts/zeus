using System;
using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.BaseLibrary.ExtensionMethods.Linq;
using Zeus.ContentTypes;
using Zeus.Integrity;
using Zeus.Linq;
using Zeus.Security;
using Zeus.Web;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins.ManageZones
{
	[DirectMethodProxyID(Alias = "ManageZones", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class ManageZonesUserControl : PluginUserControlBase
	{
		[DirectMethod]
		public void OpenZonesPanel(int id)
		{
			ContentItem contentItem = Engine.Persister.Get(id);

			TreePanel treePanel = new TreePanel
			{
				ID = "trpManageZones",
				Icon = Icon.ApplicationSideBoxes,
				Title = contentItem.Title + " Zones",
				Region = Region.East,
				AutoScroll = true,
				UseArrows = true,
				RootVisible = false,
				BodyStyle="padding-top:5px",
				Split = true,
				MinWidth = 150,
				Width = 175,
				MaxWidth = 300,
				Collapsible = true,
				CloseAction = CloseAction.Close,
				EnableDD = true
			};

			IMainInterface mainInterface = (IMainInterface) Page;
			mainInterface.Viewport.Items.Add(treePanel);

			treePanel.Tools.Add(new Tool(ToolType.Close, "panel.hide(); " + mainInterface.Viewport.ClientID + ".doLayout();", string.Empty));

			// Setup tree top toolbar.
			var topToolbar = new Toolbar();
			treePanel.TopBar.Add(topToolbar);

			var addButton = new Button
			{
				ID = "addButton",
				Icon = Icon.Add,
				Disabled = true
			};
			addButton.ToolTips.Add(new ToolTip { Html = "Add New Widget" });
			addButton.Listeners.Click.Fn = "function(button) { top.zeus.reloadContentPanel('New Widget', " + treePanel.ClientID + ".getSelectionModel().getSelectedNode().attributes['newItemUrl']); }";
			topToolbar.Items.Add(addButton);

			treePanel.Listeners.Click.Handler = "Ext.getCmp('" + addButton.ClientID + "').setDisabled(node.isLeaf());";

			if (Engine.SecurityManager.IsAuthorized(contentItem, Context.User, Operations.Delete))
			{
				var deleteButton = new Button
				{
					ID = "deleteButton",
					Icon = Icon.Delete,
					Disabled = true
				};
				deleteButton.ToolTips.Add(new ToolTip { Html = "Delete Widget" });
				deleteButton.Listeners.Click.Fn = string.Format(@"
					function(button)
					{{
						var node = {0}.getSelectionModel().getSelectedNode();
						stbStatusBar.showBusy('Deleting...');
						Ext.net.DirectMethods.ManageZones.DeleteWidget(node.id,
						{{
							url : '{1}',
							success: function()
							{{
								node.parentNode.removeChild(node);
								stbStatusBar.setStatus({{ text: 'Deleted Widget', iconCls: '', clear: true }});
								top.zeus.reloadContentPanel('Preview', '{2}');
							}}
						}});
					}}",
					treePanel.ClientID,
					Engine.AdminManager.GetAdminDefaultUrl(),
					contentItem.Url);
				topToolbar.Items.Add(deleteButton);

				treePanel.Listeners.Click.Handler += "Ext.getCmp('" + deleteButton.ClientID + "').setDisabled(!node.isLeaf());";
			}

			treePanel.Listeners.MoveNode.Handler = string.Format(@"
				{0}.showBusy();
				Ext.net.DirectMethods.ManageZones.MoveWidget(node.id, newParent.id, index,
				{{
					url: '{1}',
					success: function() {{ {0}.setStatus({{ text: 'Moved Widget', iconCls: '', clear: true }}); }}
				}})",
				mainInterface.StatusBar.ClientID,
				Engine.AdminManager.GetAdminDefaultUrl());

			ContentType definition = Zeus.Context.ContentTypes.GetContentType(contentItem.GetType());

			var rootNode = new TreeNode { Expanded = true };
			foreach (var availableZone in definition.AvailableZones)
			{
				var zoneNode = new TreeNode
				{
					NodeID = availableZone.ZoneName,
					Expanded = true,
					IconFile = Utility.GetCooliteIconUrl(Icon.ApplicationSideList),
					Text = availableZone.Title,
					Leaf = false
				};
				zoneNode.CustomAttributes.Add(new ConfigItem("newItemUrl",
					GetPageUrl(typeof(ManageZonesUserControl), "Zeus.Admin.Plugins.NewItem.Default.aspx") + "?selected=" + contentItem.Path + "&zoneName=" + availableZone.ZoneName,
					ParameterMode.Value));
				rootNode.Nodes.Add(zoneNode);

				foreach (var widget in GetItemsInZone(contentItem, availableZone))
				{
					var widgetNode = new TreeNode
					{
						NodeID = widget.ID.ToString(),
						Leaf = true,
						Text = widget.Title,
						IconFile = widget.IconUrl,
						Href = string.Format("javascript: top.zeus.reloadContentPanel('Edit', '{0}');",
							GetPageUrl(typeof(ManageZonesUserControl), "Zeus.Admin.Plugins.EditItem.Default.aspx") + "?selected=" + widget.Path)
					};
					zoneNode.Nodes.Add(widgetNode);
				}
			}
			treePanel.Root.Add(rootNode);

			treePanel.Render();
		}

		protected string GetPageUrl(Type type, string resourcePath)
		{
			return Zeus.Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(type.Assembly, resourcePath);
		}

		protected IEnumerable<WidgetContentItem> GetItemsInZone(ContentItem contentItem, AvailableZoneAttribute availableZone)
		{
			return Zeus.Context.Current.ContentManager.GetWidgets(contentItem, availableZone.ZoneName);
		}

		[DirectMethod]
		public void DeleteWidget(int id)
		{
			ContentItem parent = Engine.Persister.Get(id).Parent;
			ContentItem item = Engine.Persister.Get(id);
			Zeus.Context.Persister.Delete(item);
		}

		[DirectMethod]
		public void MoveWidget(int id, string destinationZone, int pos)
		{
			WidgetContentItem contentItem = (WidgetContentItem) Engine.Persister.Get(id);

			// Change zone name.
			contentItem.ZoneName = destinationZone;

			// Update sort order based on new pos in sibling widgets.
			IList<ContentItem> siblingWidgets = contentItem.Parent.Children
				.OfType<WidgetContentItem>()
				.InZone(destinationZone)
				.Cast<ContentItem>()
				.ToList();
			Utility.MoveToIndex(siblingWidgets, contentItem, pos);

			// Get index of widget before this one in full Children collection.
			IList<ContentItem> siblings = contentItem.Parent.Children;
			WidgetContentItem previousWidget = siblingWidgets.Previous(contentItem) as WidgetContentItem;
			int index = (previousWidget != null) ? siblings.IndexOf(previousWidget) : 0;
			Utility.MoveToIndex(siblings, contentItem, index);

			foreach (ContentItem updatedItem in Utility.UpdateSortOrder(siblings))
				Zeus.Context.Persister.Save(updatedItem);

			Engine.Persister.Save(contentItem);
		}
	}
}