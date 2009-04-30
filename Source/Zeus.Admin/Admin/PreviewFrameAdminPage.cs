using System;

namespace Zeus.Admin
{
	public class PreviewFrameAdminPage : AdminPage
	{
		protected override void OnPreInit(EventArgs e)
		{
			MasterPageFile = "~/Admin/PreviewFrame.Master";
			base.OnPreInit(e);
		}
	}
}
