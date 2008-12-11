using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Items
{
	[ContentType(Description = "Product brands")]
	[RestrictParents(typeof(BrandContainer))]
	public class Brand : BaseContentItem
	{
		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
