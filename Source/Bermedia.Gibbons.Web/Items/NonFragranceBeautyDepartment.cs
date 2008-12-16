using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Department", SortOrder = 300, Description = "e.g. Children & Baby, 9 West, Bath, etc. ")]
	[RestrictParents(typeof(StartPage), typeof(NonFragranceBeautyDepartment))]
	public class NonFragranceBeautyDepartment : BaseDepartment
	{

	}
}
