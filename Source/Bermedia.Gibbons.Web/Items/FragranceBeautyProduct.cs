using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;
using System.Web.UI.WebControls;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Product", Description = "e.g. Calvin Klein Striped Socks, Must de Cartier Eau de Toilette")]
	[RestrictParents(typeof(FragranceBeautyCategory))]
	public class FragranceBeautyProduct : StandardProduct
	{
		public override Brand Brand
		{
			get { return FindBrand((FragranceBeautyCategory) this.Parent); }
			set { throw new NotImplementedException(); }
		}

		private static Brand FindBrand(FragranceBeautyCategory category)
		{
			if (category.Brand != null)
				return category.Brand;
			else if (category.Parent != null && category.Parent is FragranceBeautyCategory)
				return FindBrand((FragranceBeautyCategory) category.Parent);
			else
				throw new InvalidOperationException("Could not find brand in any parent categories for this product");
		}
	}
}
