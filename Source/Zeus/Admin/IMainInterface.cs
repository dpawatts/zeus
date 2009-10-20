using Coolite.Ext.Web;

namespace Zeus.Admin
{
	public interface IMainInterface
	{
		StatusBar StatusBar { get; }
		ViewPort ViewPort { get; }
		BorderLayout BorderLayout { get; }

		void LoadUserControls(string[] virtualPaths);
	}
}