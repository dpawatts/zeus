using Coolite.Ext.Web;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(BasePage))]
	public class Sitemap : BasePage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Sitemap); }
		}
	}
}