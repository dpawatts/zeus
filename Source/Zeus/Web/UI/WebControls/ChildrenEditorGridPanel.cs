using Ext.Net;

namespace Zeus.Web.UI.WebControls
{
	public class ChildrenEditorGridPanel : GridPanel
	{
		public ChildrenEditorGridPanel()
		{
			Header = false;
			AutoWidth = false;
			AutoHeight = false;
			AutoScroll = true;
			Width = 400;
			Height = 300;
		}

		protected override void CreateChildControls()
		{
			/*Toolbar topToolbar = new Toolbar();
			topToolbar.Items.Add(new Button("Add Item") { Icon = Icon.Add });
			topToolbar.Items.Add(new Button("Remove Item(s)") { Icon = Icon.Delete });
			TopBar.Add(topToolbar);*/

			ColumnModel.Columns.Add(new RowNumbererColumn());
			ColumnModel.Columns.Add(new Column
			{
				ColumnID = "ID",
				Header = "ID",
				DataIndex = "ID",
				Width = 100
			});
			ColumnModel.Columns.Add(new Column
			{
				ColumnID = "Title",
				Header = "Title",
				DataIndex = "Title",
				Width = 200
			});

			base.CreateChildControls();
		}
	}
}