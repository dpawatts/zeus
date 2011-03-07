using Ext.Net;
using Zeus.ContentTypes;
using System.Web.UI;

namespace Zeus.Web.UI
{
	public class TabPanelAttribute : EditorContainerAttribute
	{
		public string Title { get; set; }

		public TabPanelAttribute(string name, string title, int sortOrder)
			: base(name, sortOrder)
		{
			Title = title;
		}

		public override Control AddTo(Control container)
		{
			// If the current container doesn't already contain a TabControl, create one now.
			CustomTabPanel tabControl = container.FindControl("TabControl") as CustomTabPanel;
			if (tabControl == null)
			{
				tabControl = new CustomTabPanel { ID = "TabControl" };
				if (container is ContentPanel)
					((ContentPanel) container).ContentControls.Add(tabControl);
				else
					container.Controls.Add(tabControl);
			}

			Panel tabItem = new Panel
			{
				AutoScroll = true,
				AutoHeight = true,
				AutoWidth = true,
				ID = "tabItem" + Name,
				Title = Title,
				BodyStyle = "padding:5px"
			};
			tabControl.Items.Add(tabItem);
			return tabItem;
		}
	}

	public class CustomTabPanel : TabPanel, INamingContainer
	{
		
	}
}
