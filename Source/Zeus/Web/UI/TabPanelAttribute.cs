using System;
using Zeus.ContentTypes;
using System.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.Web.UI
{
	public class TabPanelAttribute : EditorContainerAttribute
	{
		public string Title
		{
			get;
			set;
		}

		public TabPanelAttribute(string name, string title, int sortOrder)
			: base(name, sortOrder)
		{
			this.Title = title;
		}

		public override Control AddTo(Control container)
		{
			TabPanel tabPanel = new TabPanel();
			tabPanel.ID = this.Name;
			tabPanel.ToolTip = this.Title;
			container.Controls.Add(tabPanel);
			return tabPanel;
		}
	}
}
