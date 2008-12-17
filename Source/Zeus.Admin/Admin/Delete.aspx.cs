using System;
using Isis.Web;

namespace Zeus.Admin
{
	public partial class Delete : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ContentItem parent = this.SelectedItem.Parent;
			Zeus.Context.Persister.Delete(this.SelectedItem);
			Refresh(parent, AdminFrame.Both, false);
		}
	}
}
