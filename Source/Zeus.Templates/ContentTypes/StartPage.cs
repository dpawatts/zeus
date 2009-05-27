using System.Collections.Generic;
using Zeus.Design.Editors;
using Zeus.FileSystem;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Start Page", "BaseStartPage", Installer = Installation.InstallerHints.PreferredStartPage)]
	[RestrictParents(typeof(RootItem))]
	public class StartPage : BasePage, IFileSystemContainer
	{
		protected override string IconName
		{
			get { return "page_world"; }
		}
	}
}