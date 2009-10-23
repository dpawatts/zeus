using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Spark.FileSystem;
using Spark.Web.Mvc;
using Zeus.BaseLibrary.DependencyInjection;
using Zeus.Web.Routing;

namespace Zeus.Web.Mvc.Modules
{
	public abstract class WebPackageBase : IWebPackage
	{
		public abstract void Register(IKernel container, ICollection<RouteBase> routes, ICollection<IViewEngine> viewEngines);

		protected void RegisterStandardArea(IKernel container, ICollection<RouteBase> routes, ICollection<IViewEngine> viewEngines, string areaName)
		{
			var assembly = GetType().Assembly;
			RegisterStandardComponents(container, assembly, areaName);
			RegisterStandardRoutes(routes, assembly, areaName);
			RegisterStandardViewFolders(viewEngines, assembly, areaName);
		}

		public void RegisterStandardComponents(IKernel container, Assembly assembly, string areaName)
		{
			DependencyInjectionUtility.RegisterAllComponentsTransient<IController>(
				container, assembly, t => GetControllerName(t, areaName));
		}

		private static string GetControllerName(Type type, string areaName)
		{
			string name = type.Name.ToLowerInvariant();

			if (name.EndsWith("controller"))
				name = name.Substring(0, name.IndexOf("controller"));

			name = areaName.ToLowerInvariant() + "." + name;

			return name;
		}

		public void RegisterStandardRoutes(ICollection<RouteBase> routes, Assembly assembly, string areaName)
		{
			routes.Add(new Route("{area}/{controller}/{action}",
				new RouteValueDictionary(new { controller = "home", action = "index" }),
				new RouteValueDictionary(new { area = areaName }),
				new MvcRouteHandler()));

			routes.Add(new Route("content/{area}/{*resource}",
				new RouteValueDictionary(),
				new RouteValueDictionary(new { area = areaName }),
				new EmbeddedContentRouteHandler(assembly, assembly.GetName().Name + ".Mvc.Content")));
		}

		public void RegisterStandardViewFolders(ICollection<IViewEngine> viewEngines, Assembly assembly, string areaName)
		{
			var viewFolder = new EmbeddedViewFolder(assembly, assembly.GetName().Name + ".Mvc.Views");
			var sparkViewFactory = viewEngines.OfType<SparkViewFactory>().First();

			sparkViewFactory.ViewFolder = sparkViewFactory.ViewFolder
				.Append(new SubViewFolder(viewFolder, areaName))
				.Append(new SubViewFolder(viewFolder, "Shared\\" + areaName));
		}
	}
}