using Zeus.Web.Mvc;

namespace Zeus.AddIns.Forums.Mvc
{
	public class ForumsAreaRegistration : StandardAreaRegistration
	{
		public const string AREA_NAME = "Forums";

		public override string AreaName
		{
			get { return AREA_NAME; }
		}
	}
}