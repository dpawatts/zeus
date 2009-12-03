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

		[ContentProperty("404 Page", 25, Description = "This page will be used if a user requests a page that does not exist.")]
		public virtual BasePage PageNotFoundPage
		{
			get { return GetDetail<BasePage>("PageNotFoundPage", null); }
			set { SetDetail("PageNotFoundPage", value); }
		}
	}
}