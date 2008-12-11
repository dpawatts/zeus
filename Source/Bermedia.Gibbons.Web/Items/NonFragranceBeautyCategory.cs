using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Items
{
	[ContentType("Category", Description = "e.g. Men's Business Attire")]
	[RestrictParents(typeof(NonFragranceBeautyDepartment), typeof(NonFragranceBeautyCategory))]
	public class NonFragranceBeautyCategory : BaseCategory
	{
		protected override string IconName
		{
			get { return "tag_red"; }
		}
	}
}
