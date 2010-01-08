using System;
using System.Collections.Generic;
using System.Linq;
using Coolite.Ext.Web;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.AddIns.Blogs.Services;
using Zeus.Admin;

namespace Zeus.AddIns.Blogs.Admin.Plugins.ModerateComments
{
	public partial class Default : PreviewFrameAdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Ext.IsAjaxRequest)
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

		protected void cboFilterStatus_Select(object sender, AjaxEventArgs e)
		{
			RefreshData();
		}

		protected void cboFilterType_Select(object sender, AjaxEventArgs e)
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

		protected void SubmitSpam(object sender, AjaxEventArgs e)
		{
			string json = e.ExtraParams["Values"];
			if (string.IsNullOrEmpty(json))
				return;

			Dictionary<string, string>[] commentRows = JSON.Deserialize<Dictionary<string, string>[]>(json);
			foreach (Dictionary<string, string> row in commentRows)
			{
				int feedbackID = Convert.ToInt32(row["ID"]);

				// Handle values.
				FeedbackItem feedbackItem = Engine.Persister.Get<FeedbackItem>(feedbackID);
				
				// Send to anti-spam service as not spam.
				//Engine.Resolve<IAntiSpamService>().
				feedbackItem.Status = FeedbackItemStatus.Spam;
				Engine.Persister.Save(feedbackItem);
			}

			//scriptManager.AddScript("Ext.Msg.alert('Submitted', 'Please see source code how to handle submitted data');");
			RefreshData();
		}

		protected void ApproveFeedback(object sender, AjaxEventArgs e)
		{
			string json = e.ExtraParams["Values"];
			if (string.IsNullOrEmpty(json))
				return;

			Dictionary<string, string>[] commentRows = JSON.Deserialize<Dictionary<string, string>[]>(json);
			foreach (Dictionary<string, string> row in commentRows)
			{
				int feedbackID = Convert.ToInt32(row["ID"]);

				// Handle values.
				FeedbackItem feedbackItem = Engine.Persister.Get<FeedbackItem>(feedbackID);
				feedbackItem.Status = FeedbackItemStatus.Approved;
				Engine.Persister.Save(feedbackItem);
			}

			//scriptManager.AddScript("Ext.Msg.alert('Submitted', 'Please see source code how to handle submitted data');");
			RefreshData();
		}
	}
}
