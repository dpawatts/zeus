using Ext.Net;
using Zeus.ContentTypes;
using Zeus.Integrity;

namespace Zeus
{
	[ContentType("Root Item", Installer = Installation.InstallerHints.PreferredRootPage)]
	[RestrictParents(AllowedTypes.None)]
	public class RootItem : DataContentItem, IRootItem
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.PageGear); }
		}
	}
}