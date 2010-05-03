using System;
using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Admin;

namespace Zeus.AddIns.Blogs.Admin.Plugins.ModerateComments
{
	public partial class Default : PreviewFrameAdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!ExtNet.IsAjaxRequest)
			{
				cboFilterStatus.SelectedItem.Value = cboFilterStatus.Items[0].Value;
				cboFilterType.SelectedItem.Value = cboFilterType.Items[0].Value;

				RefreshData();
			}
		}

		protected void exsDataStore_RefreshData(object sender, StoreRefreshDataEventArgs e)
		{
			RefreshData();
		}

		protected void cboFilterStatus_Select(object sender, DirectEventArgs e)
		{
			RefreshData();
		}

		protected void cboFilterType_Select(object sender, DirectEventArgs e)
		{
			RefreshData();
		}

		private void RefreshData()
		{
			var feedbackItems = Find.EnumerateAccessibleChildren(SelectedItem).OfType<FeedbackItem>();
			switch (cboFilterStatus.SelectedItem.Value)
			{
				case "Pending" :
					feedbackItems = feedbackItems.Where(fi => fi.Status == FeedbackItemStatus.Pending);
					break;
				case "Approved":
					feedbackItems = feedbackItems.Where(fi => fi.Status == FeedbackItemStatus.Approved);
					break;
				case "Spam":
					feedbackItems = feedbackItems.Where(fi => fi.Status == FeedbackItemStatus.Spam);
					break;
			}
			switch (cboFilterType.SelectedItem.Value)
			{
				case "Comments":
					feedbackItems = feedbackItems.Where(fi => fi is Comment);
					break;
				case "Pingbacks":
					feedbackItems = feedbackItems.Where(fi => fi is Pingback);
					break;
				case "All":
					break;
			}
			exsDataStore.DataSource = feedbackItems.ToArray();
			exsDataStore.DataBind();
		}

		private void UpdateFeedbackItemStatus(DirectEventArgs e, Action<FeedbackItem> callback)
		{
			RowSelectionModel sm = (RowSelectionModel) gpaComments.SelectionModel.Primary;
			foreach (SelectedRow row in sm.SelectedRows)
			{
				int feedbackID = Convert.ToInt32(row.RecordID);

				FeedbackItem feedbackItem = Engine.Persister.Get<FeedbackItem>(feedbackID);
				callback(feedbackItem);
				Engine.Persister.Save(feedbackItem);
			}
			sm.SelectedRows.Clear();
			sm.UpdateSelection();
		}

		protected void SubmitSpam(object sender, DirectEventArgs e)
		{
			UpdateFeedbackItemStatus(e, fi => fi.Status = FeedbackItemStatus.Spam);
			RefreshData();
		}

		protected void ApproveFeedback(object sender, DirectEventArgs e)
		{
			UpdateFeedbackItemStatus(e, fi =>
			{
				// TODO - if feedback item was previously marked as spam, need
				// to let spam service know this is not spam.
				fi.Status = FeedbackItemStatus.Approved;
			});
			RefreshData();
		}
	}
}
