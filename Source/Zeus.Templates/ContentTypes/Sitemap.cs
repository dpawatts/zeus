using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	public class Sitemap : BasePage
	{
		protected override string IconName
		{
			get { return "sitemap"; }
		}
	}
}