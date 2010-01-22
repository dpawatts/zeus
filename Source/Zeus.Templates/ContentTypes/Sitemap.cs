using Ext.Net;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(WebsiteNode), typeof(Page))]
	public class Sitemap : BasePage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Sitemap); }
		}
	}
}