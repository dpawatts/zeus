﻿using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Category", Description = "e.g. Women's Fragrance")]
	[RestrictParents(typeof(FragranceBeautyDepartment))]
	public class FragranceBeautyCategory : BaseFragranceBeautyCategory
	{
		
	}
}
