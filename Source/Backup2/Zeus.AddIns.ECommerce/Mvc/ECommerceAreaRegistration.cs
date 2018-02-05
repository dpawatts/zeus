using Zeus.Web.Mvc;

namespace Zeus.AddIns.ECommerce.Mvc
{
	public class ECommerceAreaRegistration : StandardAreaRegistration
	{
		public const string AREA_NAME = "ECommerce";

		public override string AreaName
		{
			get { return AREA_NAME; }
		}
	}
}