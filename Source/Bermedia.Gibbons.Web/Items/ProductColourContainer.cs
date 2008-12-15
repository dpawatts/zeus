using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Product Colour Container", Description = "Container for product colours")]
	[RestrictParents(typeof(RootItem))]
	public class ProductColourContainer : BaseContentItem
	{
		public ProductColourContainer()
		{
			this.Name = "ProductColours";
			this.Title = "Product Colors";
		}

		protected override string IconName
		{
			get { return "ipod_cast"; }
		}
	}
}
