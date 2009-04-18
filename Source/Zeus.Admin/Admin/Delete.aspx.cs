using System;

namespace Zeus.Admin
{
	public partial class Delete : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Zeus.Context.UrlParser.IsRootOrStartPage(SelectedItem))
				throw new InvalidOperationException("Cannot delete root item or home page");

			if (SelectedItem.GetType().Name.Contains("Department"))
				throw new InvalidOperationException("Cannot delete department");

			ContentItem parent = this.SelectedItem.Parent;
			Zeus.Context.Persister.Delete(this.SelectedItem);
			Refresh(parent, AdminFrame.Both, false);
		}
	}
}
