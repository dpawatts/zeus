using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Items
{
	[ContentType(Description = "e.g. Women's Trends")]
	[RestrictParents(typeof(Page), typeof(Department))]
	public class Page : StructuralPage
	{
		[EditableFreeTextArea("Navigation Text", 100)]
		public string NavigationText
		{
			get { return GetDetail<string>("NavigationText", string.Empty); }
			set { SetDetail<string>("NavigationText", value); }
		}

		[EditableFreeTextArea("Text", 110)]
		public string Text
		{
			get { return GetDetail<string>("Text", string.Empty); }
			set { SetDetail<string>("Text", value); }
		}

		protected override string IconName
		{
			get { return "tag_green"; }
		}
	}
}
