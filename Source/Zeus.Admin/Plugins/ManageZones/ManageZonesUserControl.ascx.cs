using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Ext.Net;
using Zeus.Admin.Plugins.EditItem;
using Zeus.BaseLibrary.Web;
using Zeus.ContentTypes;
using Zeus.Integrity;
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
				CloseAction = CloseAction.Close
			};

			IMainInterface mainInterface = (IMainInterface) Page;
			mainInterface.Viewport.Items.Add(treePanel);

			treePanel.Tools.Add(new Tool(ToolType.Close, "panel.hide(); " + mainInterface.Viewport.ClientID + ".doLayout();", string.Empty));

			// Setup tree top toolbar.
			Toolbar topToolbar = new Toolbar();
			treePanel.TopBar.Add(topToolbar);

			Button addButton = new Button { ID = "addButton", Icon = Icon.Add };
			addButton.Disabled = true;
			addButton.ToolTips.Add(new ToolTip { Html = "Add New Widget" });
			addButton.Listeners.Click.Fn = "function(node) { top.zeus.reloadContentPanel('New Widget', " + treePanel.ClientID + ".getSelectionModel().getSelectedNode().attributes['newItemUrl']); }";
			topToolbar.Items.Add(addButton);

			treePanel.Listeners.Click.Handler = "Ext.getCmp('" + addButton.ClientID + "').setDisabled(node.isLeaf());";

			ContentType definition = Zeus.Context.ContentTypes.GetContentType(contentItem.GetType());

			var rootNode = new TreeNode { Expanded = true };
			foreach (var availableZone in definition.AvailableZones)
			{
				var zoneNode = new TreeNode
				{
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
	}
}