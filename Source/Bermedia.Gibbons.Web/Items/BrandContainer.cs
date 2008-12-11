using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Items
{
	[ContentType(Description = "Container for brands")]
	[RestrictParents(typeof(RootItem))]
	public class BrandContainer : BaseContentItem
	{
		protected override string IconName
		{
			get { return "ipod_cast"; }
		}
	}
}
