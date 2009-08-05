using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.Web.Mvc;
using Zeus.Engine;
using Zeus.Web.Mvc.Modules;

namespace Zeus.Web.Mvc
{
	public class SparkApplication
	{
		public void RegisterViewEngine(ICollection<IViewEngine> engines)
		{
			SparkEngineStarter.RegisterViewEngine(engines);
		}

		public void RegisterPackages(ContentEngine engine, ICollection<RouteBase> routes, ICollection<IViewEngine> engines)
		{
			var manager = engine.Resolve<IWebPackageManager>();
			manager.LocatePackages();
			manager.RegisterPackages(routes, engines);
		}
	}
}
