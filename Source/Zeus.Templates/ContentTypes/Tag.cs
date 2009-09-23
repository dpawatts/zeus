using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(TagGroup))]
	public class Tag : BasePage
	{
		protected override string IconName
		{
			get { return "tag_red"; }
		}
	}
}