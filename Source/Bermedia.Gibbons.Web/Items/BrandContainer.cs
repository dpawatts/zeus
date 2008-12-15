using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Brand Container", Description = "Container for brands")]
	[RestrictParents(typeof(RootItem))]
	public class BrandContainer : BaseContentItem
	{
		public BrandContainer()
		{
			this.Name = this.Title = "Brands";
		}

		protected override string IconName
		{
			get { return "ipod_cast"; }
		}
	}
}
