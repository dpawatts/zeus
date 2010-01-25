using System.Web.UI;
using Ext.Net;

namespace Zeus.Admin
{
	public interface IMainInterface
	{
		StatusBar StatusBar { get; }
		Viewport Viewport { get; }

		void LoadUserControls(string[] virtualPaths);
		void AddControl(Control control);
	}
}