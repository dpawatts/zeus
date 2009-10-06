using System;
using System.Linq;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;
using Zeus.Integrity;

namespace Zeus.Admin.RecycleBin
{
	public partial class Default : PreviewFrameAdminPage
	{
		#region Properties

		protected IRecycleBinHandler RecycleBin
		{
			get { return Zeus.Context.Current.Resolve<IRecycleBinHandler>(); }
		}

		#endregion

		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			hlCancel.NavigateUrl = CancelUrl();
			if (!IsPostBack)
				ReBind();
			cvRestore.IsValid = true;
			btnEmpty.Enabled = SelectedItem.GetChildren().Any();

			base.OnLoad(e);
		}

		protected void btnEmpty_Click(object sender, EventArgs e)
		{
			foreach (ContentItem child in SelectedItem.GetChildren())
				Zeus.Context.Persister.Delete(child);
			ReBind();

			Refresh(SelectedItem, AdminFrame.Navigation, false);
		}

		protected void grvRecycleBinItems_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			int itemID = Convert.ToInt32(e.CommandArgument);
			ContentItem item = Zeus.Context.Persister.Get(itemID);

			switch (e.CommandName)
			{
				case "Restore" :
					try
					{
						RecycleBin.Restore(item);
						ReBind();
					}
					catch (NameOccupiedException)
					{
						cvRestore.IsValid = false;
					}
					Refresh(item, AdminFrame.Navigation, false);
					break;
				case "Delete" :
					Zeus.Context.Persister.Delete(item);
					ReBind();
					Refresh(SelectedItem, AdminFrame.Navigation, false);
					break;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.view.css");
			base.OnPreRender(e);
		}

		private void ReBind()
		{
			grvRecycleBinItems.DataSource = SelectedItem.GetChildren();
			grvRecycleBinItems.DataBind();
		}

		#endregion

		protected void grvRecycleBinItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			
		}
	}
}
