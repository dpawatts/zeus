using System;

namespace Zeus.Admin
{
	public abstract class AdminPage : System.Web.UI.Page
	{
		private const string RefreshPreviewFormat = @"
if (window.top.zeus)
	window.top.zeus.refreshPreview('{1}');
else
	window.location = '{1}';";
		private const string RefreshBothFormat = @"
if (window.top.zeus)
	window.top.zeus.refresh('{0}', '{1}');
else
	window.location = '{1}';";

		public void Refresh(ContentItem contentItem, bool justPreview)
		{
			string script = string.Format((justPreview) ? RefreshPreviewFormat : RefreshBothFormat,
				"/admin/navigation/tree.aspx?selecteditem=" + contentItem.ID, // 0
				contentItem.Url // 1
			);

			this.ClientScript.RegisterClientScriptBlock(
				typeof(AdminPage),
				"AddRefreshEditScript",
				script, true);
		}
	}
}
