using System;
using Coolite.Ext.Web;

namespace Zeus.Admin
{
	public partial class Children : PreviewFrameAdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			// Render grid toolbar action plugin buttons.
			foreach (IGridToolbarPlugin toolbarPlugin in Engine.AdminManager.GetGridToolbarPlugins())
			{
				ToolbarButton button = toolbarPlugin.GetToolbarButton(SelectedItem, gpaChildren);
				TopToolbar.Items.Add(button);

				toolbarPlugin.ModifyGrid(button, gpaChildren);
			}

			base.OnInit(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Ext.IsAjaxRequest)
				RefreshData();
		}

		protected void exsDataStore_RefreshData(object sender, StoreRefreshDataEventArgs e)
		{
			RefreshData();
		}

		private void RefreshData()
		{
			exsDataStore.DataSource = SelectedItem.GetChildren();
			exsDataStore.DataBind();
		}
	}
}
