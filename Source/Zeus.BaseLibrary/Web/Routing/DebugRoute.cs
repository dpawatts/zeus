using System.Web.Routing;

namespace Zeus.BaseLibrary.Web.Routing
{
	public class DebugRoute : Route
	{
		// Fields
		private static DebugRoute singleton = new DebugRoute();

		// Methods
		private DebugRoute()
			: base("{*catchall}", new DebugRouteHandler())
		{

		}

		// Properties
		public static DebugRoute Singleton
		{
			get
			{
				return singleton;
			}
		}
	}
}