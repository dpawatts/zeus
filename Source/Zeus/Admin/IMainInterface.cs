using Coolite.Ext.Web;
using System.Web.UI;

namespace Zeus.Admin
{
	public interface IMainInterface
	{
		StatusBar StatusBar { get; }
		ViewPort ViewPort { get; }
		BorderLayout BorderLayout { get; }

		void LoadUserControls(string[] virtualPaths);
		void AddControl(Control control);
	}
}