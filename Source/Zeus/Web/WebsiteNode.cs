using Ext.Net;
using Zeus.FileSystem;
using Zeus.Integrity;
using Zeus.Web.Security;

namespace Zeus.Web
{
	[ContentType("Website", "BaseStartPage", Installer = Installation.InstallerHints.PreferredStartPage)]
	[RestrictParents(typeof(RootItem))]
	public class WebsiteNode : PageContentItem, IFileSystemContainer, ILoginContext
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

		#region Implementation of ILoginContext

		[ContentProperty("Login Page", 30, Description = "This page will be used if a user requests a page that they do not have access to.")]
		public virtual PageContentItem LoginPage
		{
			get { return GetDetail<PageContentItem>("LoginPage", null); }
			set { SetDetail("LoginPage", value); }
		}

		public string LoginUrl
		{
			get { return (LoginPage != null) ? LoginPage.Url : null; }
		}

		#endregion
	}
}