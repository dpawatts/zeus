using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Tag Group", Description = "Defines a tag group that pages can be associated with.")]
	[RestrictParents(typeof(BasePage))]
	public class TagGroup : BasePage
	{
		protected override string IconName
		{
			get { return "tag_blue"; }
		}
	}
}