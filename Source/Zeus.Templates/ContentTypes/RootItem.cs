using Zeus.ContentTypes;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Root Item", Installer = Installation.InstallerHints.PreferredRootPage)]
	[RestrictParents(AllowedTypes.None)]
	public class RootItem : BaseContentItem, IRootItem
	{
		protected override string IconName
		{
			get { return "page_gear"; }
		}
	}
}