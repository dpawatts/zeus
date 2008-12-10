using System;
using Zeus.ContentTypes;
using System.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.Web.UI
{
	public class TabPanelAttribute : EditorContainerAttribute
	{
		public TabPanelAttribute(string name, string title, int sortOrder)
		{

		}

		public override Control AddTo(Control container)
		{
			TabPanel tabPanel = new TabPanel();
			container.Controls.Add(tabPanel);
			return tabPanel;
		}
	}
}
