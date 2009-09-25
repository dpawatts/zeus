using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.Web.Mvc;
using Spark.Web.Mvc.Descriptors;
using Zeus.Engine;
using Zeus.Web.Mvc.Descriptors;
using Zeus.Web.Mvc.Modules;

namespace Zeus.Web.Mvc
{
	public class SparkApplication
	{
		public void RegisterViewEngine(ICollection<IViewEngine> engines)
		{
			var services = SparkEngineStarter.CreateContainer();
			services.AddFilter(new MobileDeviceDescriptorFilter());
			SparkEngineStarter.RegisterViewEngine(engines, services);
		}

		public void RegisterPackages(ContentEngine engine, ICollection<RouteBase> routes, ICollection<IViewEngine> engines)
		{
			var manager = engine.Resolve<IWebPackageManager>();
			manager.LocatePackages();
			manager.RegisterPackages(routes, engines);
		}
	}
}
