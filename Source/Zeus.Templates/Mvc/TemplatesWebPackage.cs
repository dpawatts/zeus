using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Spark.FileSystem;
using Spark.Web.Mvc;
using Zeus.Web.Mvc.Modules;
using Zeus.Web.Routing;

namespace Zeus.Templates.Mvc
{
	public class TemplatesWebPackage : WebPackageBase
	{
		public const string AREA_NAME = "Templates";

		public override void Register(IKernel container, ICollection<RouteBase> routes, ICollection<IViewEngine> viewEngines)
		{
			RegisterStandardArea(container, routes, viewEngines, AREA_NAME);

			var assembly = GetType().Assembly;

			routes.Add(new Route("assets/{*resource}",
				new RouteValueDictionary(),
				new RouteValueDictionary(),
				new EmbeddedContentRouteHandler(assembly, assembly.GetName().Name + ".Mvc.DefaultTemplate.Assets")));

			var viewFolder = new EmbeddedViewFolder(assembly, assembly.GetName().Name + ".Mvc.DefaultTemplate.Views");
			var sparkViewFactory = viewEngines.OfType<SparkViewFactory>().First();

			sparkViewFactory.ViewFolder = sparkViewFactory.ViewFolder
				.Append(viewFolder);
		}
	}
}