using Ext.Net;
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

		protected override Icon Icon
		{
			get { return Icon.World; }
		}
	}
}