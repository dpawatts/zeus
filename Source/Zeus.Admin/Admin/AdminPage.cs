using System;
using System.Web;

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

		public virtual ContentItem SelectedItem
		{
			get
			{
				ContentItem selectedItem = GetFromViewState()
					?? GetFromUrl()
					?? Zeus.Context.UrlParser.StartPage;
				return selectedItem;
			}
			set
			{
				if (value != null)
					SelectedItemID = value.ID;
				else
					SelectedItemID = 0;
			}
		}

		private int SelectedItemID
		{
			get { return (int) (ViewState["SelectedItemID"] ?? 0); }
			set { ViewState["SelectedItemID"] = value; }
		}

		public void Refresh(ContentItem contentItem, bool justPreview)
		{
			string script = string.Format((justPreview) ? RefreshPreviewFormat : RefreshBothFormat,
				"/admin/navigation/tree.aspx?selected=" + HttpUtility.UrlEncode(contentItem.Path), // 0
				contentItem.Url // 1
			);

			this.ClientScript.RegisterClientScriptBlock(
				typeof(AdminPage),
				"AddRefreshEditScript",
				script, true);
		}

		private ContentItem GetFromViewState()
		{
			if (SelectedItemID != 0)
				return Zeus.Context.Persister.Get(SelectedItemID);
			return null;
		}

		private ContentItem GetFromUrl()
		{
			string selected = Request.QueryString["selected"];
			if (!string.IsNullOrEmpty(selected))
				return Zeus.Context.Current.Resolve<Navigator>().Navigate(selected);

			return null;
		}
	}
}
