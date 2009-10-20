using System;
using Coolite.Ext.UX;
using Coolite.Ext.Web;

namespace Zeus.Admin.Plugins.Children
{
	public partial class Default : PreviewFrameAdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			// Render grid toolbar action plugin buttons.
			foreach (IGridToolbarPlugin toolbarPlugin in Engine.ResolveAll<IGridToolbarPlugin>())
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
			{
				ScriptManager.GetInstance(this).RegisterClientScriptInclude(typeof(StoreMenu), "Coolite.Ext.UX.Extensions.StoreMenu.resources.ext.ux.menu.storemenu.js");
				RefreshData();
			}
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