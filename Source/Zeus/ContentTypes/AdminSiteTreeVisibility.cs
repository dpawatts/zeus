using System;

namespace Zeus.ContentTypes
{
	[Flags]
	public enum AdminSiteTreeVisibility
	{
		Visible = 1,
		Hidden = 2,
		ChildrenHidden = 4
	}
}