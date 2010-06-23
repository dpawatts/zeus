using Zeus.Web.Mvc;

namespace Zeus.Examples.MinimalMvcExample
{
	public class WebsiteAreaRegistration : StandardAreaRegistration
	{
		public const string AREA_NAME = "Website";

		public override string AreaName
		{
			get { return AREA_NAME; }
		}
	}
}
