using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.Widgets
{
	[ContentType("Tag Cloud", Description = "Displays a list of tags, with the size of each tag set relative to how many pages reference it")]
	[AllowedZones(AllowedZones.AllNamed)]
	public class TagCloud : BaseWidget
	{
		protected override string IconName
		{
			get { return "tag_yellow"; }
		}

		[ContentProperty("Tag Group", 200)]
		public TagGroup TagGroup
		{
			get { return GetDetail<TagGroup>("TagGroup", null); }
			set { SetDetail("TagGroup", value); }
		}

		[ContentProperty("Minimum Font Size", 210)]
		public int MinFontSize
		{
			get { return GetDetail("MinFontSize", 10); }
			set { SetDetail("MinFontSize", value); }
		}

		[ContentProperty("Maximum Font Size", 220)]
		public int MaxFontSize
		{
			get { return GetDetail("MaxFontSize", 20); }
			set { SetDetail("MaxFontSize", value); }
		}
	}
}