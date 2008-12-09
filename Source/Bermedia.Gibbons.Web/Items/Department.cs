using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Items
{
	[ContentType(Description = "e.g. Children & Baby, 9 West, Bath, etc.")]
	[RestrictParents(typeof(Page), typeof(Department))]
	public class Department : StructuralPage
	{
		[HtmlTextBoxEditor("Text", 100)]
		public string Text
		{
			get { return GetDetail<string>("Text", string.Empty); }
			set { SetDetail<string>("Text", value); }
		}

		protected override string IconName
		{
			get { return "tag_purple"; }
		}
	}
}
