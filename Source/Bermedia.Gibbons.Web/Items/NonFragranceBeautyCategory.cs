﻿using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Category", Description = "e.g. Men's Business Attire")]
	[RestrictParents(typeof(NonFragranceBeautyDepartment), typeof(NonFragranceBeautyCategory))]
	public class NonFragranceBeautyCategory : BaseCategory
	{
		
	}
}