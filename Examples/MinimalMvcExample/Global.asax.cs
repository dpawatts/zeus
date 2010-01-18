using System.Web.Routing;
using Zeus.BaseLibrary.Web.Routing;

namespace Zeus.Examples.MinimalMvcExample
{
	public class Global : Zeus.Web.Mvc.MvcGlobal
	{
		protected override void OnApplicationStart(System.EventArgs e)
		{
			base.OnApplicationStart(e);
			//RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
		}
	}
}