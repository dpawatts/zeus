using System;

namespace Zeus.Admin
{
	public class PreviewFrameAdminPage : AdminPage
	{
		protected override void OnPreInit(EventArgs e)
		{
			MasterPageFile = Engine.GetServerResourceUrl(typeof(PreviewFrameAdminPage), "Zeus.Admin.PreviewFrame.master");
			base.OnPreInit(e);
		}
	}
}
