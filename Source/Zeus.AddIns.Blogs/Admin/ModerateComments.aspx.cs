using System;
using System.Collections.Generic;
using System.Linq;
using Coolite.Ext.Web;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Admin;

namespace Zeus.AddIns.Blogs.Admin
{
	public partial class ModerateComments : PreviewFrameAdminPage
	{
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
			exsDataStore.DataSource = Find.EnumerateAccessibleChildren(SelectedItem).OfType<FeedbackItem>().ToArray();
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

				// Delete.
				Engine.Persister.Delete(feedbackItem);
			}

			//scriptManager.AddScript("Ext.Msg.alert('Submitted', 'Please see source code how to handle submitted data');");
			RefreshData();
		}
	}
}
