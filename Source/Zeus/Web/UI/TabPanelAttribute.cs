using System;
using Zeus.ContentTypes;
using System.Web.UI;
using Zeus.Web.UI.WebControls;

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
			TabControl tabControl = container.FindControl("TabControl") as TabControl;
			if (tabControl == null)
			{
				tabControl = new TabControl();
				tabControl.ID = "TabControl";
				container.Controls.Add(tabControl);
			}

			TabItem tabItem = new TabItem();
			tabItem.ID = "tabItem" + Name;
			tabItem.ToolTip = Title;
			tabControl.Controls.Add(tabItem);
			return tabItem;
		}
	}
}
