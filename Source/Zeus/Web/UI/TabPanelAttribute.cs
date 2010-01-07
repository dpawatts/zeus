using System.Web.UI.WebControls;
using Coolite.Ext.Web;
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
				tabControl = new CustomTabPanel();
				tabControl.ID = "TabControl";
				if (container is ContentPanel)
					((ContentPanel) container).BodyControls.Add(tabControl);
				else
					container.Controls.Add(tabControl);
			}

			Tab tabItem = new Tab();
			tabItem.AutoScroll = true;
			tabItem.AutoHeight = true;
			tabItem.ID = "tabItem" + Name;
			tabItem.Title = Title;
			tabItem.BodyStyle = "padding:5px";
			tabControl.Tabs.Add(tabItem);
			return tabItem;
		}
	}

	public class CustomTabPanel : TabPanel, INamingContainer
	{
		
	}
}
