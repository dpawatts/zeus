using System;
using Isis.Web.Hosting;
using Zeus.Security;

[assembly: EmbeddedResourceFile("Zeus.Admin.Delete.aspx", "Zeus.Admin")]
namespace Zeus.Admin
{
	[ActionPlugin("Delete", "Delete", Operations.Delete, "NewEditDelete", 3, null, "Zeus.Admin.Delete.aspx", "selected={selected}&alert=true", Targets.Preview, "Zeus.Admin.Resources.page_delete.png")]
	[AvailableOperation(Operations.Delete, "Delete", 40)]
	public partial class Delete : PreviewFrameAdminPage
	{
		protected override void OnInit(EventArgs e)
		{
			hlCancel.NavigateUrl = CancelUrl();

			uscItemsToDelete.CurrentItem = SelectedItem;
			uscItemsToDelete.DataBind();
			uscReferencingItems.Item = SelectedItem;
			uscReferencingItems.DataBind();

			base.OnInit(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			if (SelectedItem is ContentItem && Zeus.Context.UrlParser.IsRootOrStartPage((ContentItem) SelectedItem))
			{
				csvDelete.IsValid = false;
				btnDelete.Enabled = false;
			}
			else
			{
				//if (!IsPostBack && Request["alert"] != null && Boolean.Parse(Request["alert"]))
				//	RegisterConfirmAlert();
			}
			Title = string.Format("Delete '{0}'", SelectedItem.Title);

			base.OnLoad(e);
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			ContentItem parent = SelectedItem.Parent;
			try
			{
				Zeus.Context.Persister.Delete(SelectedItem);

				if (parent != null)
					Refresh(parent, AdminFrame.Both, false);
				else
					Refresh(Zeus.Context.UrlParser.StartPage, AdminFrame.Both, false);
			}
			catch (Exception ex)
			{
				//Engine.Resolve<IErrorHandler>().Notify(ex);
				csvException.IsValid = false;
				csvException.Text = ex.ToString();
			}
		}
	}
}
