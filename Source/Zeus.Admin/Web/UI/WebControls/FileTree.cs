namespace Zeus.Admin.Web.UI.WebControls
{
	public class FileTree : Tree
	{
		public string RootPath
		{
			get { return (ViewState["RootPath"] as string ?? "~/"); }
			set { ViewState["RootPath"] = value; }
		}

		public override ContentItem RootNode
		{
			get { return Zeus.Context.Current.Resolve<Navigator>().Navigate(RootPath); }
		}
	}
}
