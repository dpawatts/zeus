using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;
using System.Web.UI.WebControls;
using Zeus.Web.UI;
using Bermedia.Gibbons.Web.Items.Details;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Product", Description = "e.g. Calvin Klein Striped Socks, Must de Cartier Eau de Toilette")]
	[RestrictParents(typeof(FragranceBeautyCategory))]
	public class FragranceBeautyProduct : StandardProduct
	{
		[ProductScentsEditor("Associated Scents", 300, ContainerName = Tabs.Colours)]
		public override DetailCollection AssociatedColours
		{
			get { return GetDetailCollection("AssociatedColours", true); }
		}

		public ProductScent Scent
		{
			get { return (this.AssociatedColours.Count > 0) ? (ProductScent) this.AssociatedColours[0] : null; }
		}

		public ProductSize Size
		{
			get { return (this.AssociatedSizes.Count > 0) ? this.AssociatedSizes[0].ProductSize : null; }
		}

		public override Brand Brand
		{
			get { return FindBrand((FragranceBeautyCategory) this.Parent); }
			set { throw new NotImplementedException(); }
		}

		private static Brand FindBrand(BaseFragranceBeautyCategory category)
		{
			if (category is FragranceBeautyBrandCategory)
				return ((FragranceBeautyBrandCategory) category).Brand;
			else if (category.Parent != null && category.Parent is BaseFragranceBeautyCategory)
				return FindBrand((BaseFragranceBeautyCategory) category.Parent);
			else
				throw new InvalidOperationException("Could not find brand in any parent categories for this product");
		}

		[LinkedItemDropDownListEditor("Strength", 295, TypeFilter = typeof(ProductStrength), ContainerName = Tabs.General)]
		public ProductStrength Strength
		{
			get { return GetDetail<ProductStrength>("Strength", null); }
			set { SetDetail<ProductStrength>("Strength", value); }
		}

		public override string DisplayTitle
		{
			get
			{
				string result = this.Brand.Title + " " + this.Title;
				if (this.Strength != null)
					result += " " + this.Strength.Title;
				if (this.Size != null)
					result += " " + this.Size.Title;
				return result;
			}
		}
	}
}
