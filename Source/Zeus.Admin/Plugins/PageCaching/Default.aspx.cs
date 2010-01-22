using System;
using Zeus.Web.Caching;

namespace Zeus.Admin.Plugins.PageCaching
{
	public partial class Default : PreviewFrameAdminPage
	{
		protected override void OnLoad(EventArgs e)
		{
			if (!IsPostBack)
			{
				chkEnableCache.Checked = SelectedItem.GetPageCachingEnabled();
				tmeCacheDuration.SelectedTime = SelectedItem.GetPageCachingDuration();
			}
			base.OnLoad(e);
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

			SelectedItem.SetPageCachingEnabled(chkEnableCache.Checked);
			SelectedItem.SetPageCachingDuration(tmeCacheDuration.SelectedTime);
			Engine.Persister.Save(SelectedItem);
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect(CancelUrl());
		}
	}
}