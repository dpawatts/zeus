namespace Zeus.Admin.Plugins
{
	public interface IToolbarPlugin
	{
		string[] RequiredScripts { get; }
		string[] RequiredUserControls { get; }
		int SortOrder { get; }
	}
}