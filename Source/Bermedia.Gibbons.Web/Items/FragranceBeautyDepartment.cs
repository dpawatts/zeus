using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Items
{
	[ContentType("Department (Fragrance & Beauty)", Description = "Special department which has category brands and not per-product brands")]
	[RestrictParents(typeof(StartPage))]
	public class FragranceBeautyDepartment : BaseDepartment
	{

	}
}
