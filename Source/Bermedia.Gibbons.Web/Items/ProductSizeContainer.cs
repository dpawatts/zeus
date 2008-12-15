using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Product Size Container", Description = "Container for product sizes")]
	[RestrictParents(typeof(RootItem))]
	public class ProductSizeContainer : BaseContentItem
	{
		public ProductSizeContainer()
		{
			this.Name = "ProductSizes";
			this.Title = "Product Sizes";
		}

		protected override string IconName
		{
			get { return "ipod_cast"; }
		}
	}
}
