using System;

namespace Zeus.Admin
{
	public partial class View : AdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			zeusItemGridView.CurrentItem = this.SelectedItem;
			base.OnInit(e);
		}

		protected void btnRefreshGrid_Click(object sender, EventArgs e)
		{
			Refresh(this.SelectedItem, AdminFrame.Navigation, true);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
		}
	}
}
