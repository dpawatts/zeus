using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Brand Category", Description = "e.g. Givenchy")]
	[RestrictParents(typeof(FragranceBeautyCategory))]
	public class FragranceBeautyBrandCategory : FragranceBeautyCategory
	{
		[LinkedItemDropDownListEditor("Brand", 200, TypeFilter = typeof(Brand), ContainerName = Tabs.General, Required = true)]
		public Brand Brand
		{
			get { return GetDetail<Brand>("Brand", null); }
			set { SetDetail<Brand>("Brand", value); }
		}
	}
}
