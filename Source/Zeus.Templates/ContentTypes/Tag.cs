using Coolite.Ext.Web;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(TagGroup))]
	public class Tag : BasePage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.TagRed); }
		}
	}
}