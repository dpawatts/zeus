using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.FileSystem;
using Spark.Web.Mvc;
using Zeus.Web.Routing;

namespace Zeus.Web.Mvc
{
	public abstract class StandardAreaRegistration : AreaRegistration
	{
		public sealed override void RegisterArea(AreaRegistrationContext context)
		{
			var assembly = GetType().Assembly;

			// Register content and asset routes.
			RegisterRoutes(context, assembly);

			// Register view folders.
			SparkViewFactory sparkViewFactory = ViewEngines.Engines.OfType<SparkViewFactory>().First();
			RegisterViewFolders(assembly, sparkViewFactory);

			RegisterArea(context, assembly, sparkViewFactory);
		}

		private void RegisterRoutes(AreaRegistrationContext context, Assembly assembly)
		{
			context.Routes.Add(new Route("assets/{area}/{*resource}",
				new RouteValueDictionary(),
				new RouteValueDictionary(new { area = AreaName }),
				new EmbeddedContentRouteHandler(assembly, assembly.GetName().Name + ".Mvc.Assets")));
		}

		private void RegisterViewFolders(Assembly assembly, SparkViewFactory sparkViewFactory)
		{
			var viewFolder = new EmbeddedViewFolder(assembly, assembly.GetName().Name + ".Mvc.Views");

			sparkViewFactory.ViewFolder = sparkViewFactory.ViewFolder
				.Append(new SubViewFolder(viewFolder, AreaName))
				.Append(new SubViewFolder(viewFolder, "Shared\\" + AreaName));
		}

		protected virtual void RegisterArea(AreaRegistrationContext context, Assembly assembly,
			SparkViewFactory sparkViewFactory)
		{
			
		}
	}
}