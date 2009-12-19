namespace Zeus.Admin.Plugins.FileManager
{
	public class FileManagerMainInterfacePlugin : MainInterfacePluginBase
	{
		public override string[] RequiredUserControls
		{
			get { return new[] { GetPageUrl(GetType(), "Zeus.Admin.Plugins.FileManager.FileManagerUserControl.ascx") }; }
		}
	}
}