using System;
using System.Reflection;
using Ninject.Modules;
using System.Web.Mvc;
using Zeus.BaseLibrary.DependencyInjection;
using Zeus.Web.Mvc.Html;
using Zeus.Web.Mvc.Modules;

namespace Zeus.Web.Mvc
{
	public class MvcModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ITemplateRenderer>().To<TemplateRenderer>().InSingletonScope();
			Bind<IControllerMapper>().To<ControllerMapper>().InSingletonScope();
			Bind<IControllerFactory>().To<ModularControllerFactory>().InSingletonScope();
			Bind<IWebPackageManager>().To<WebPackageManager>().InSingletonScope();

			RegisterAllControllersIn(typeof(MvcModule).Assembly);
		}

		/// <summary>
		/// Registers all controllers in the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly to search for controllers.</param>
		private void RegisterAllControllersIn(Assembly assembly)
		{
			DependencyInjectionUtility.RegisterAllComponentsTransient<IController>(Kernel, assembly, GetControllerName);
		}

		private static string GetControllerName(Type type)
		{
			string name = type.Name.ToLowerInvariant();

			if (name.EndsWith("controller"))
				name = name.Substring(0, name.IndexOf("controller"));

			return name;
		}
	}
}