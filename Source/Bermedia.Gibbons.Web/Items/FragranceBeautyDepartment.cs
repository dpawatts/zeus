using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Department (Fragrance & Beauty)", SortOrder = 300, Description = "Special department which has category brands and not per-product brands")]
	[RestrictParents(typeof(StartPage))]
	public class FragranceBeautyDepartment : BaseDepartment
	{
		public override ContentItem GetChild(string childName)
		{
			if (childName.Equals("shop-online", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "shop-online";
				return this;
			}
			else if (childName.Equals("scents", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "scents";
				return this;
			}
			else if (childName.Equals("brands", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "brands";
				return this;
			}
			else if (childName.Equals("brand", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "brand";
				return this;
			}
			else if (childName.Equals("exclusives", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "exclusives";
				return this;
			}
			return base.GetChild(childName);
		}

		public override string TemplateUrl
		{
			get
			{
				switch (Action)
				{
					case "shop-online":
						return "~/UI/Views/FragranceBeautyShopOnline.aspx";
					case "scents":
						return "~/UI/Views/FragranceBeautyScents.aspx";
					case "brands":
						return "~/UI/Views/FragranceBeautyBrands.aspx";
					case "brand":
						return "~/UI/Views/FragranceBeautyBrand.aspx";
					case "exclusives":
						return "~/UI/Views/FragranceBeautyExclusives.aspx";
					default:
						return base.TemplateUrl;
				}
			}
		}
	}
}
