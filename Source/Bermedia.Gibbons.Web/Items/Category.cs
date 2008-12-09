using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Items
{
	[ContentType(Description = "e.g. Men's Business Attire")]
	[RestrictParents(typeof(Department), typeof(Category))]
	public class Category : StructuralPage
	{
		protected override string IconName
		{
			get { return "tag_red"; }
		}
	}
}
