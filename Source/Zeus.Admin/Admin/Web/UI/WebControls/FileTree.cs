using System;

namespace Zeus.Admin.Web.UI.WebControls
{
	public class FileTree : Tree
	{
		public override ContentItem RootNode
		{
			get { return Zeus.Context.Current.Resolve<Navigator>().Navigate("~/Upload/"); }
		}
	}
}
