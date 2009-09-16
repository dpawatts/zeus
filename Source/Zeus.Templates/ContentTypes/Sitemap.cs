using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(BasePage))]
	public class Sitemap : BasePage
	{
		protected override string IconName
		{
			get { return "sitemap"; }
		}
	}
}