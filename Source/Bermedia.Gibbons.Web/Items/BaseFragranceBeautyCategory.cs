using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[RestrictParents(typeof(FragranceBeautyDepartment))]
	public abstract class BaseFragranceBeautyCategory : BaseCategory
	{
		protected override string TemplateName
		{
			get { return "FragranceBeautyCategory"; }
		}
	}
}
