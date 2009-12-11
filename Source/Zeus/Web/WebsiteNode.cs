using Coolite.Ext.Web;
using Zeus.FileSystem;
using Zeus.Integrity;

namespace Zeus.Web
{
	[ContentType("Website", "BaseStartPage", Installer = Installation.InstallerHints.PreferredStartPage)]
	[RestrictParents(typeof(RootItem))]
	public class WebsiteNode : PageContentItem, IFileSystemContainer
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.PageWorld); }
		}

		[ContentProperty("404 Page", 25, Description = "This page will be used if a user requests a page that does not exist.")]
		public virtual PageContentItem PageNotFoundPage
		{
			get { return GetDetail<PageContentItem>("PageNotFoundPage", null); }
			set { SetDetail("PageNotFoundPage", value); }
		}
	}
}