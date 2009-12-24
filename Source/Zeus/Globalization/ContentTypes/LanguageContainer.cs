using Zeus.BaseLibrary.Web.UI;
using Zeus.ContentTypes;
using Zeus.Integrity;
using Zeus.Security;

namespace Zeus.Globalization.ContentTypes
{
	[ContentType("Language Container")]
	[ContentTypeAuthorizedRoles(RoleNames.Administrators)]
	[RestrictParents(typeof(IRootItem))]
	[Translatable(false)]
	public class LanguageContainer : DataContentItem
	{
		public LanguageContainer()
		{
			Title = "Languages";
			Name = "languages";
		}

		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(LanguageContainer), "Zeus.Web.Resources.Icons.world.png"); }
		}
	}
}