using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Spark.Web.Mvc;
using Zeus.Web.Mvc.Modules;

namespace Zeus.Web.Mvc
{
	public class SparkApplication
	{
		public void RegisterComponents(IWindsorContainer container)
		{
			container.AddComponent("zeus.mvc.controllerMapper", typeof(IControllerMapper), typeof(ControllerMapper));

			container
				.Register(Component
					.For<IControllerFactory>()
					.ImplementedBy<ModularControllerFactory>()
					.LifeStyle.Singleton)

				.Register(Component
					.For<IWebPackageManager>()
					.ImplementedBy<WebPackageManager>()
					.LifeStyle.Transient)

				.Register(AllTypes
					.Of<IController>()
					.FromAssembly(typeof(SparkApplication).Assembly)
					.Configure(component => component
						.Named(component.ServiceType.Name.ToLowerInvariant())
						.LifeStyle.Transient));
		}

		public void RegisterViewEngine(ICollection<IViewEngine> engines)
		{
			SparkEngineStarter.RegisterViewEngine(engines);
		}

		public void RegisterPackages(IWindsorContainer container, ICollection<RouteBase> routes, ICollection<IViewEngine> engines)
		{
			var manager = container.Resolve<IWebPackageManager>();
			manager.LocatePackages();
			manager.RegisterPackages(routes, engines);
			container.Release(manager);
		}
	}
}
