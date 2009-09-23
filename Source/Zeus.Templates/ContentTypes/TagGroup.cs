using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Tag Group")]
	[RestrictParents(typeof(ITagRoot))]
	public class TagGroup : BasePage
	{
		protected override string IconName
		{
			get { return "tag_blue"; }
		}
	}
}