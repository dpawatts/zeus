using System;

namespace Zeus.Admin
{
	public class PreviewFrameAdminPage : AdminPage
	{
		protected override void OnPreInit(EventArgs e)
		{
			MasterPageFile = "~/admin/PreviewFrame.master";
			base.OnPreInit(e);
		}
	}
}
