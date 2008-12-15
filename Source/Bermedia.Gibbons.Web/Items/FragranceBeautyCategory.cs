using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Category", Description = "e.g. Givenchy")]
	[RestrictParents(typeof(FragranceBeautyDepartment), typeof(FragranceBeautyCategory))]
	public class FragranceBeautyCategory : BaseCategory
	{
		[LinkedItemDropDownListEditor("Brand", 200, TypeFilter = typeof(Brand), ContainerName = Tabs.General)]
		public Brand Brand
		{
			get { return GetDetail<Brand>("Brand", null); }
			set { SetDetail<Brand>("Brand", value); }
		}
	}
}
