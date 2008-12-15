using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Associated Size")]
	[RestrictParents(typeof(StandardProduct))]
	public class ProductSizeLink : BaseContentItem
	{
		public override string Title
		{
			get { return (this.ProductSize != null) ? this.ProductSize.Title : "[None]"; }
		}

		[LinkedItemDropDownListEditor("Product Size", 200, TypeFilter = typeof(ProductSize), Required = true)]
		public ProductSize ProductSize
		{
			get { return GetDetail<ProductSize>("ProductSize", null); }
			set { SetDetail<ProductSize>("ProductSize", value); }
		}

		[TextBoxEditor("Vendor Style Number", 210)]
		public string VendorStyleNumber
		{
			get { return GetDetail<string>("VendorStyleNumber", null); }
			set { SetDetail<string>("VendorStyleNumber", value); }
		}

		[TextBoxEditor("Regular Price", 220)]
		public decimal? RegularPrice
		{
			get { return GetDetail<decimal?>("RegularPrice", null); }
			set { SetDetail<decimal?>("RegularPrice", value); }
		}

		[TextBoxEditor("Sale Price", 230)]
		public decimal? SalePrice
		{
			get { return GetDetail<decimal?>("SalePrice", null); }
			set { SetDetail<decimal?>("SalePrice", value); }
		}

		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
