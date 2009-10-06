using System.Linq;
using Isis.Web.UI;
using Zeus.ContentTypes;
using Zeus.Installation;
using Zeus.Integrity;
using Zeus.Web.Hosting;

namespace Zeus.Admin.RecycleBin
{
	[ContentType("Recycle Bin", "RecycleBinContainer", Installer = InstallerHints.NeverRootOrStartPage)]
	[AllowedChildren(typeof(ContentItem))]
	[ContentTypeAuthorizedRoles(Roles = new string[0])]
	[RestrictParents(typeof(IRootItem))]
	[NotThrowable]
	public class RecycleBinContainer : ContentItem, INode, IRecycleBin
	{
		[ContentProperty("Enabled", 80)]
		public virtual bool Enabled
		{
			get { return GetDetail("Enabled", true); }
			set { SetDetail("Enabled", value); }
		}

		[ContentProperty("Number of days to keep deleted items", 100)]
		public virtual int KeepDays
		{
			get { return GetDetail("KeepDays", 31); }
			set { SetDetail("KeepDays", value); }
		}

		string INode.PreviewUrl
		{
			get { return Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(typeof(RecycleBinContainer).Assembly, "Zeus.Admin.RecycleBin.Default.aspx") + "?selected=" + Path; }
		}

		public override string IconUrl
		{
			get
			{
				string iconName = Children.Any() ? "bin" : "bin_closed";
				return WebResourceUtility.GetUrl(typeof(RecycleBinContainer), "Zeus.Admin.Assets.Images.Icons." + iconName + ".png");
			}
		}
	}
}